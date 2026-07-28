namespace TradeFinder.DataManager;

/// <summary>
/// Предоставляет методы для получения и поиска локаций.
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Получает список локаций указанного типа.
    /// При необходимости результат может быть ограничен родительской локацией.
    /// </summary>
    /// <param name="locationTypeCode">
    /// Код типа локации (например: COUNTRY, REGION, CITY).
    /// </param>
    /// <param name="parentLocationId">
    /// Идентификатор родительской локации. Если <see langword="null"/>,
    /// будут возвращены все локации указанного типа.
    /// </param>
    /// <returns>Список найденных локаций.</returns>
    Task<List<Location>> GetByType(string locationTypeCode, Guid? parentLocationId = null);
}