using AndcultureCode.CSharp.Core;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Data;
using AndcultureCode.CSharp.Core.Interfaces.Entity;
using AndcultureCode.CSharp.Core.Models;
using AndcultureCode.CSharp.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sandbox.Infrastructure.Data.SqlServer.Repositories
{
    public class Repository<T> : IRepository<T>
        where T: Entity
    {
        #region Properties

        public int? CommandTimeout
        {
            get
            {
                if (Context != null && Context is DbContext)
                {
                    return DbContext.Database.GetCommandTimeout();
                }
                return null;
            }
            set
            {
                if (Context != null && Context is DbContext)
                {
                    DbContext.Database.SetCommandTimeout(value);
                }
            }
        }

        public  IContext      Context   { get; private set; }
        private DbContext     DbContext { get => (DbContext)Context; }
        public  IQueryable<T> Query     { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor injecting the data context
        /// </summary>
        /// <param name="context"></param>
        public Repository(IContext context)
        {
            Context = context;
            Query   = context.Query<T>();
        }

        #endregion

        #region IRepository Implementation

        /// <summary>
        /// Uses BulkCreate
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="createdById"></param>
        public virtual IResult<List<T>> BulkCreate(IEnumerable<T> entities, long? createdById = default(long?))
        {
            throw new NotImplementedException();
        }

        public virtual IResult<List<T>> BulkCreateDistinct<TKey>(IEnumerable<T> entities, Func<T, TKey> property, long? createdById = default(long?)) => BulkCreate(entities.GroupBy(property).Select(x => x.First()), createdById);

        /// <summary>
        /// Uses BulkDelete
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        public IResult<bool> BulkDelete(IEnumerable<T> entities, long? deletedById = null, bool soft = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uses BulkUpdate
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public virtual IResult<bool> BulkUpdate(IEnumerable<T> entities, long? updatedBy = default(long?))
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createdById"></param>
        /// <returns></returns>
        public virtual IResult<T> Create(T entity, long? createdById = default(long?))
        {
            var result = new Result<T>();

            try
            {
                if (entity is ICreatable)
                {
                    if (createdById.HasValue)
                    {
                        ((ICreatable)entity).CreatedById = createdById;
                    }
                    ((ICreatable)entity).CreatedOn   = DateTimeOffset.UtcNow;
                }

                Context.Add(entity);
                Context.DetectChanges(); // Note: New to EF Core, #SaveChanges, #Add and other methods do NOT automatically call DetectChanges
                Context.SaveChanges();

                result.ResultObject = entity;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), $"{ex.Message} -- {ex.InnerException?.Message}");
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="createdById"></param>
        public virtual IResult<List<T>> Create(IEnumerable<T> entities, long? createdById = default(long?))
        {
            var result = new Result<List<T>> { ResultObject = new List<T>() };

            try
            {
                var numInserted = 0;

                foreach (var entity in entities)
                {
                    if (entity is ICreatable)
                    {
                        if (createdById.HasValue)
                        {
                            ((ICreatable)entity).CreatedById = createdById;
                        }
                        ((ICreatable)entity).CreatedOn   = DateTimeOffset.UtcNow;
                    }

                    Context.Add(entity);
                    result.ResultObject.Add(entity);

                    // Save in batches of 100, if there are at least 100 entities.
                    if (++numInserted >= 100)
                    {
                        numInserted = 0;

                        Context.DetectChanges(); // Note: New to EF Core, #SaveChanges, #Add and other methods do NOT automatically call DetectChanges
                        Context.SaveChanges();
                    }
                }

                // Save whatever is left over.
                Context.DetectChanges(); // Note: New to EF Core, #SaveChanges, #Add and other methods do NOT automatically call DetectChanges
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), $"{ex.Message} -- {ex.InnerException?.Message}");
            }

            return result;
        }

        public IResult<List<T>> CreateDistinct<TKey>(IEnumerable<T> items, Func<T, TKey> property, long? createdById = null) => Create(items.GroupBy(property).Select(x => x.First()), createdById);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        public virtual IResult<bool> Delete(long id, long? deletedById = default(long?), bool soft = true)
        {
            IResult<T> findResult;
            if (soft == false)
            {
                findResult = FindById(id, true);
            }
            else
            {
                findResult = FindById(id);
            }
            if (findResult.HasErrors)
            {
                return new Result<bool>
                {
                    Errors       = findResult.Errors,
                    ResultObject = false
                };
            }

            return Delete(findResult.ResultObject, deletedById, soft);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        public virtual IResult<bool> Delete(T entity, long? deletedById = default(long?), bool soft = true)
        {
            var result = new Result<bool> { ResultObject = false };

            try
            {
                if (entity == null)
                {
                    result.AddError("Delete", $"{entity.GetType()} does not exist.");
                    return result;
                }

                if (soft && !(entity is IDeletable))
                {
                    result.AddError("Delete", "In order to perform a soft delete, the object must implement the IDeletable interface.");
                    return result;
                }

                if (soft)
                {
                    if (deletedById.HasValue)
                    {
                        ((IDeletable)entity).DeletedById = deletedById;
                    }
                    ((IDeletable)entity).DeletedOn   = DateTimeOffset.UtcNow;
                }
                else
                {
                    Context.Delete(entity);
                }

                Context.SaveChanges();
                result.ResultObject = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Find all filtered, sorted and paged
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public virtual IResult<IQueryable<T>> FindAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null,
            int? skip = null, int? take = null, bool? ignoreQueryFilters = false, bool asNoTracking = false)
        {
            var result = new Result<IQueryable<T>>();

            try
            {
                result.ResultObject = GetQueryable(filter, orderBy, includeProperties, skip, take, ignoreQueryFilters, asNoTracking);
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Find all filtered, sorted and paged
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        public virtual IResult<IList<T>> FindAllCommitted(
            Expression<Func<T, bool>> filter                  = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties                          = null,
            int? skip                                         = null,
            int? take                                         = null,
            bool? ignoreQueryFilters                          = false)
        {
            var result = new Result<IList<T>>();

            try
            {
                result.ResultObject = GetQueryable(filter, orderBy, includeProperties, skip, take, ignoreQueryFilters).ToList();
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, bool? ignoreQueryFilters = false)
        {
            return FindById(
                id:                 id,
                ignoreQueryFilters: ignoreQueryFilters.Value,
                asNoTracking:       false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, bool ignoreQueryFilters, bool asNoTracking)
        {
            var result = new Result<T>();

            try
            {
                if (ignoreQueryFilters)
                {
                    Query = Query.IgnoreQueryFilters();
                }

                if (asNoTracking)
                {
                    result.ResultObject = Query.AsNoTracking().FirstOrDefault(e => e.Id == id);
                }
                else
                {
                    result.ResultObject = Query.FirstOrDefault(e => e.Id == id);
                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, bool? ignoreQueryFilters = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var result = new Result<T>();

            try
            {
                var query = Query;

                if (ignoreQueryFilters.HasValue && ignoreQueryFilters.Value)
                {
                    query = query.IgnoreQueryFilters();
                }

                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }

                result.ResultObject = query.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, params Expression<Func<T, object>>[] includeProperties)
        {
            var result = new Result<T>();

            try
            {
                var query = Query;

                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }

                result.ResultObject = query.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, params string[] includeProperties)
        {
            var result = new Result<T>();

            try
            {
                var query = Query;

                foreach (var property in includeProperties)
                {
                    if (!string.IsNullOrEmpty(property))
                    {
                        query = query.Include(property);
                    }
                }

                result.ResultObject = query.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IResult<T> FindById(long id, bool? ignoreQueryFilters = false, params string[] includeProperties)
        {
            var result = new Result<T>();

            try
            {
                var query = Query;

                if (ignoreQueryFilters.HasValue && ignoreQueryFilters.Value)
                {
                    query = query.IgnoreQueryFilters();
                }

                foreach (var property in includeProperties)
                {
                    if (!string.IsNullOrEmpty(property))
                    {
                        query = query.Include(property);
                    }
                }

                result.ResultObject = query.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual IResult<bool> Restore(long id)
        {
            var result = new Result<bool> { ResultObject = false };

            try
            {
                var findResult = FindById(id);
                if (findResult.HasErrors)
                {
                    result.AddErrors(findResult.Errors);
                    return result;
                }

                var restoreResult = Restore(findResult.ResultObject);
                if (restoreResult.HasErrors)
                {
                    result.AddErrors(restoreResult.Errors);
                    return result;
                }

                result.ResultObject = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual IResult<bool> Restore(T entity)
        {
            var result = new Result<bool> { ResultObject = false };

            try
            {
                if (entity is IDeletable)
                {
                    ((IDeletable)entity).DeletedById = null;
                    ((IDeletable)entity).DeletedOn   = null;
                }

                Context.Update(entity);
                Context.SaveChanges();

                result.ResultObject = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public virtual IResult<bool> Update(T entity, long? updatedBy = default(long?))
        {
            var result = new Result<bool> { ResultObject = false };

            try
            {
                if (entity is IUpdatable)
                {
                    ((IUpdatable)entity).UpdatedById = updatedBy;
                    ((IUpdatable)entity).UpdatedOn   = DateTimeOffset.UtcNow;
                }

                Context.Update(entity);
                Context.SaveChanges();

                result.ResultObject = true;
            }
            catch (Exception ex)
            {
                result.AddError(ex.GetType().ToString(), ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Updates an enumerable of entities in the database.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="updatedBy"></param>
        /// <returns>True if entities updated without any exceptions. False if an exception was thrown.</returns>
        public virtual IResult<bool> Update(IEnumerable<T> entities, long? updatedBy = default(long?)) => Do<bool>.Try((r) =>
        {
            var numUpdated = 0;

            foreach (var entity in entities)
            {
                if (entity is IUpdatable)
                {
                    ((IUpdatable)entity).UpdatedById = updatedBy;
                    ((IUpdatable)entity).UpdatedOn   = DateTimeOffset.UtcNow;
                }

                Context.Update(entity);

                // Save in batches of 100, if there are at least 100 entities.
                if (++numUpdated >= 100)
                {
                    numUpdated = 0;

                    Context.SaveChanges();
                }
            }

            // Save whatever is left over.
            Context.SaveChanges();

            return true;
        })
        .Result;

        #endregion

        #region Protected Methods

        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null,
            int? skip = null, int? take = null, bool? ignoreQueryFilters = false, bool asNoTracking = false)
        {
            includeProperties = includeProperties ?? string.Empty;
            var query         = Query;

            if (ignoreQueryFilters.HasValue && ignoreQueryFilters.Value)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            foreach (var includeProperty in includeProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        #endregion
    }
}
