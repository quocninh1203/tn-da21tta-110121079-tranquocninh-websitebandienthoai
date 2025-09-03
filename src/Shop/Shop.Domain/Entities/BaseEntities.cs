using System.Reflection;

namespace Shop.Domain.Entities
{
    public class BaseEntities<TKey>
    {
        public TKey Id { get; set; }
        public void UpdateWith<T>(T updatedEntity) where T : BaseEntities<TKey>
        {
            if (updatedEntity == null)
                throw new ArgumentNullException(nameof(updatedEntity));

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Bỏ qua trường Id
                if (property.Name == nameof(Id))
                    continue;

                if (!property.CanRead || !property.CanWrite)
                    continue;

                var newValue = property.GetValue(updatedEntity);
                var currentValue = property.GetValue(this);

                bool shouldUpdate = newValue != null;

                if (property.PropertyType == typeof(string))
                {
                    var strValue = newValue as string;
                    shouldUpdate = !string.IsNullOrWhiteSpace(strValue);
                }

                if (shouldUpdate)
                {
                    property.SetValue(this, newValue);
                }
            }
        }
    }
}
