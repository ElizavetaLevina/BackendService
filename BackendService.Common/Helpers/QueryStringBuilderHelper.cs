using System.Text;

namespace BackendService.Common.Helpers
{
    /// <summary>
    /// Строитель url с параметрами запроса
    /// </summary>
    /// <param name="baseUrl">базовый url без параметров запроса</param>
    public class QueryStringBuilderHelper(string baseUrl)
    {
        private readonly StringBuilder _queryBuilder = new(baseUrl);

        /// <summary>
        /// Добавление параметра запроса
        /// </summary>
        /// <param name="name">имя параметра</param>
        /// <param name="param">значение параметра</param>
        /// <returns>тот же экземпляр строителя для цепочки вызовов</returns>
        public QueryStringBuilderHelper AddParam(string name, string? param)
        {
            if (string.IsNullOrWhiteSpace(param)) return this;

            _queryBuilder.Append(name).Append("=").Append(param).Append("&");
            return this;
        }

        /// <summary>
        /// Добавление параметр с API ключом
        /// </summary>
        /// <param name="apiKey">значение API ключа</param>
        /// <returns>тот же экземпляр строителя для цепочки вызовов</returns>
        public QueryStringBuilderHelper AddApiKey(string apiKey)
        {
            _queryBuilder.Append("apiKey").Append("=").Append(apiKey);
            return this;
        }

        /// <summary>
        /// Возвращает построенный url
        /// </summary>
        /// <returns>полный url с параметрами запроса</returns>
        public string Build() => _queryBuilder.ToString();
    }
}
