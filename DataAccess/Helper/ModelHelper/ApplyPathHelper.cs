using DataAccess.Entities;
using DataAccess.Helper.ModelHelper.Interface;
using System.Reflection;

namespace DataAccess.Helper.LazyDI.ModelHelper
{
    public static class ApplyPathHelper
    {
        public static ResultEntity ApplyPatch<TEntity, TPatch>(this TEntity entity, TPatch patch, Action<PatchOperation<TEntity>>? options = null)
            where TEntity : class, new()
            where TPatch : IPatch, new()
        {
            var patchOperation = new PatchOperation<TEntity>();
            options = options ?? new Action<PatchOperation<TEntity>>(entity => { }); 
            options(patchOperation);
            return ApplyPatch(entity, patch, patchOperation);
        }
        private static ResultEntity ApplyPatch<TEntity, TPatch>(this TEntity entity, TPatch patch, PatchOperation<TEntity> patchOperation)
            where TEntity : class, new()
            where TPatch : IPatch, new()
        {
            var result = new ResultEntity() { Res = true, ResObject = entity };
            var modelColumns = patchOperation.ModelColumnEntities;
            var keys = patchOperation.ModelKeyColumnEntities;
            var ignoreItems = patchOperation.ModeIgnoreColumnEntities;
            foreach (var patchItem in patch.PATCHES)
            {
                try
                {
                    string? name = "";
                    name = modelColumns.FirstOrDefault(x => x.Name.ToLower() == patchItem.KEY.ToLower())?.Name;
                    if (patchOperation.IsPatchUseSnake != patchOperation.IsModelUseSnake)
                    {
                        name = modelColumns.FirstOrDefault(x => x.Name.AllToLower() == patchItem.KEY.AllToLower())?.Name;
                    }
                    if (string.IsNullOrEmpty(name))
                        continue;
                    if (keys.Any(x => x.Name == name) || ignoreItems.Any(x => x.Name == name))
                        continue;
                    var property = typeof(TEntity).GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (property == null)
                        continue;
                    if (property.CanWrite)
                    {
                        var convertedValue = Convert.ChangeType(patchItem.VALUE, property.PropertyType);
                        property.SetValue(entity, convertedValue);
                    }
                }
                catch
                {
                    result.Res = false;
                    return result;
                }
            }
            return result;
        }
    }
}
