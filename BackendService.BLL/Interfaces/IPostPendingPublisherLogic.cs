namespace BackendService.BLL.Interfaces
{
	public interface IPostPendingPublisherLogic
	{
		/// <summary>
		/// Отправка постов на модерацию и обновление их статусов
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Задача отправки постов</returns>
		Task PublishMessage(CancellationToken token = default);
	}
}
