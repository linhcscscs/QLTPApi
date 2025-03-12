using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccess.Helper.LazyDI.ModelHelper
{
    public class PatchOperation<TEntity> where TEntity : class, new()
    {
        private List<ModelColumnEntity>? modelColumnEntities = null;
        private List<ModelColumnEntity>? keys = null;
        private List<ModelColumnEntity>? ignores = null;
        public class ModelColumnEntity
        {
            public string Name { get; set; } = "";
            public bool IsKey { get; set; } = false;
            public bool IsIgnorePatch { get; set; } = false;
        }
        public bool IsPatchUseSnake { get; set; } = true;
        public bool IsModelUseSnake { get; set; } = true;
        public Expression<Func<TEntity, object>>? PatchByKeyExpression { get; set; }
        public Expression<Func<TEntity, object>>? IgnorePatchByFieldExpression { get; set; } = null;
        public Expression<Func<TEntity, object>>? OnlyApplyPatchByFieldExpression { get; set; } = null;
        public List<ModelColumnEntity> ModelColumnEntities
        {
            get
            {
                if (modelColumnEntities != null && modelColumnEntities.Any()) return modelColumnEntities;
                modelColumnEntities = new List<ModelColumnEntity>();
                var allProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in allProperties)
                {
                    modelColumnEntities.Add(new ModelColumnEntity { Name = property.Name });
                }
                #region Key
                if (PatchByKeyExpression != null)
                {
                    var type = PatchByKeyExpression.Body.GetType();
                    if (PatchByKeyExpression.Body is NewExpression newExpression && newExpression.Members != null)
                    {
                        foreach (var member in newExpression.Members)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == member.Name);
                            if (modelColumnEntitie != null)
                            {
                                modelColumnEntitie.IsKey = true;
                                modelColumnEntitie.IsIgnorePatch = true;
                            }
                        }
                    }
                    else if (PatchByKeyExpression.Body is MemberExpression memberExpression)
                    {
                        var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == memberExpression.Member.Name);
                        if (modelColumnEntitie != null)
                        {
                            modelColumnEntitie.IsKey = true;
                            modelColumnEntitie.IsIgnorePatch = true;
                        }
                    }
                    else if (PatchByKeyExpression.Body is UnaryExpression unaryExpression)
                    {
                        if (unaryExpression.Operand is MemberExpression unaryMemberExpression)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == unaryMemberExpression.Member.Name);
                            if (modelColumnEntitie != null)
                            {
                                modelColumnEntitie.IsKey = true;
                                modelColumnEntitie.IsIgnorePatch = true;
                            }
                        }
                    }
                }
                #endregion
                #region OnlyApplyPatchByFieldExpression
                if (OnlyApplyPatchByFieldExpression != null)
                {
                    foreach (var modelColumnEntity in modelColumnEntities)
                    {
                        modelColumnEntity.IsIgnorePatch = true;
                    }
                    if (OnlyApplyPatchByFieldExpression.Body is NewExpression newExpression && newExpression.Members != null)
                    {
                        foreach (var member in newExpression.Members)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == member.Name);
                            if (modelColumnEntitie != null)
                                modelColumnEntitie.IsIgnorePatch = false;
                        }
                    }
                    else if (OnlyApplyPatchByFieldExpression.Body is MemberExpression memberExpression)
                    {
                        var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == memberExpression.Member.Name);
                        if (modelColumnEntitie != null)
                            modelColumnEntitie.IsIgnorePatch = false;
                    }
                    else if (OnlyApplyPatchByFieldExpression.Body is UnaryExpression unaryExpression)
                    {
                        if (unaryExpression.Operand is MemberExpression unaryMemberExpression)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == unaryMemberExpression.Member.Name);
                            if (modelColumnEntitie != null)
                                modelColumnEntitie.IsIgnorePatch = false;
                        }
                    }
                }
                #endregion
                #region IgnorePatchByFieldExpression
                if (IgnorePatchByFieldExpression != null)
                {
                    if (IgnorePatchByFieldExpression.Body is NewExpression newExpression && newExpression.Members != null)
                    {
                        foreach (var member in newExpression.Members)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == member.Name);
                            if (modelColumnEntitie != null)
                                modelColumnEntitie.IsIgnorePatch = true;
                        }
                    }
                    else if (IgnorePatchByFieldExpression.Body is MemberExpression memberExpression)
                    {
                        var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == memberExpression.Member.Name);
                        if (modelColumnEntitie != null)
                            modelColumnEntitie.IsIgnorePatch = true;
                    }
                    else if (IgnorePatchByFieldExpression.Body is UnaryExpression unaryExpression)
                    {
                        if (unaryExpression.Operand is MemberExpression unaryMemberExpression)
                        {
                            var modelColumnEntitie = modelColumnEntities.FirstOrDefault(x => x.Name == unaryMemberExpression.Member.Name);
                            if (modelColumnEntitie != null)
                                modelColumnEntitie.IsIgnorePatch = true;
                        }
                    }
                }
                #endregion
                return modelColumnEntities;
            }
        }
        public List<ModelColumnEntity> ModelKeyColumnEntities
        {
            get
            {
                if (keys == null)
                {
                    keys = ModelColumnEntities.Where(x => x.IsKey).ToList();
                }
                return keys;
            }
        }
        public List<ModelColumnEntity> ModeIgnoreColumnEntities
        {
            get
            {
                if (ignores == null)
                {
                    ignores = ModelColumnEntities.Where(x => x.IsIgnorePatch).ToList();
                }
                return ignores;
            }
        }
    }
}
