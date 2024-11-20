using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Software_Taller_y_Repuestos.Extensions
{
    public static class SessionExtensions
    {
        // Guardar un objeto en la sesión
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Obtener un objeto de la sesión
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
