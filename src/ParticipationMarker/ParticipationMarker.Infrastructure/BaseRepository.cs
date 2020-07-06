using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace ParticipationMarker.Infrastrucutre
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : ITableEntity, new()
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly CloudTable _cloudTable;

        protected BaseRepository(ICloudStorageSettings cloudStorageSettings, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(cloudStorageSettings.ConnectionString);
            _cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = _cloudTableClient.GetTableReference(tableName);
        }

        public async Task<TableResult> CreateAsync(T entity)
        {
            var tableOperation = TableOperation.InsertOrReplace(entity);
            return await _cloudTable.ExecuteAsync(tableOperation);
        }

        public async Task<TableResult> DeleteAsync(T entity)
        {
            var tableOperation = TableOperation.Delete(entity);
            return await _cloudTable.ExecuteAsync(tableOperation);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _cloudTable.CreateQuery<T>().AsQueryable().Where(expression);
        }

        public async Task<TableResult> UpdateAsync(T entity)
        {
            var tableOperation = TableOperation.Replace(entity);
            return await _cloudTable.ExecuteAsync(tableOperation);
        }

        public T FindById(string id)
        {
            return Find(x => x.RowKey == id).FirstOrDefault();
        }
    }
}