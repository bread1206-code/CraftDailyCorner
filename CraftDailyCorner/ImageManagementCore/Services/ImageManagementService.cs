using CraftDailyCorner.ImageManagementCore.Interfaces;
using CraftDailyCorner.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CraftDailyCorner.ImageManagementCore.Services
{
    public abstract class ImageManagementService<T>
        where T : class, IEntityImage
    {
        protected readonly CraftDailyCornerContext _db;
        protected readonly DbSet<T> _dbSet;

        protected ImageManagementService(CraftDailyCornerContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        // =========================
        // 基本取得（可被覆寫）
        // =========================

        public virtual async Task<List<IEntityImage>> GetImagesAsync(string entityId)
        {
            var result = await _dbSet
                .Where(x => x.EntityID == entityId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return result.Cast<IEntityImage>().ToList();
        }

        // =========================
        // 共用新增
        // =========================

        protected async Task AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        // =========================
        // 取得下一排序值
        // =========================

        protected async Task<byte> GetNextSortOrderAsync(
            Expression<Func<T, bool>> predicate)
        {
            var maxSort = await _dbSet
                .Where(predicate)
                .MaxAsync(x => (byte?)x.SortOrder)
                ?? 0;

            return (byte)(maxSort + 1);
        }

        // =========================
        // 共用排序更新
        // =========================

        protected async Task UpdateSortInternalAsync(
            List<T> entities,
            List<long> orderedIds,
            Action<T, byte> applySort)
        {
            for (int i = 0; i < orderedIds.Count; i++)
            {
                var entity = entities
                    .FirstOrDefault(x => x.ImageID == orderedIds[i]);

                if (entity != null)
                {
                    applySort(entity, (byte)(i + 1));
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}