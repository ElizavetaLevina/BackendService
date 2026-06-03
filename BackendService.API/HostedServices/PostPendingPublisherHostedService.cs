using BackendService.BLL.Interfaces;

namespace BackendService.API.HostedServices
{
	/// <summary>
	/// Фоновый процесс для отправки постов со статусом Pending в очередь модерации.
	/// Запускается каждые 5 секунд, создаёт scope для получения Scoped-сервисов.
	/// </summary>
	/// <param name="serviceProvider">Провайдер сервисов для создания scope</param>
	/// <param name="logger">Логгер</param>
	public class PostPendingPublisherHostedService(IServiceProvider serviceProvider, ILogger<PostPendingPublisherHostedService> logger) : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
		private readonly ILogger<PostPendingPublisherHostedService> _logger = logger;

		/// <summary>
		/// Основной цикл фонового процесса. Каждые 5 секунд получает батч постов
		/// со статусом Pending и отправляет их в очередь модерации.
		/// При недоступности RabbitMQ ожидает его восстановления.
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача фонового процесса</returns>
		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					using var scope = _serviceProvider.CreateScope();
					var logic = scope.ServiceProvider.GetRequiredService<IPostPendingPublisherLogic>();

					await logic.PublishMessage(token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка в фоновом процессе публикации");
				}

				await Task.Delay(TimeSpan.FromSeconds(5), token);
			}
		}
	}
}
