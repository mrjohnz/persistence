//-----------------------------------------------------------------------
// <copyright file="EntityPersistenceTest.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   using Atlas.Persistence;

   using NUnit.Framework;

   public class EntityPersistenceTest<TEntity>
      where TEntity : class, new()
   {
      private readonly IUnitOfWork unitOfWork;
      private readonly IUnitOfWorkFactory unitOfWorkFactory;
      private readonly IEntityComparer entityComparer;
      private readonly Dictionary<string, IProperty<TEntity>> properties = new Dictionary<string, IProperty<TEntity>>();
      private readonly List<IKeyProperty<TEntity>> keyProperties = new List<IKeyProperty<TEntity>>();

      private IIdentityKeyProperty<TEntity> identityKeyProperty;

      public EntityPersistenceTest(IUnitOfWork unitOfWork, IUnitOfWorkFactory unitOfWorkFactory, IEntityComparer entityComparer)
      {
         ThrowIf.ArgumentIsNull(unitOfWork, "unitOfWork");
         ThrowIf.ArgumentIsNull(unitOfWorkFactory, "unitOfWorkFactory");

         this.unitOfWork = unitOfWork;
         this.unitOfWorkFactory = unitOfWorkFactory;
         this.entityComparer = entityComparer;
      }

      public EntityPersistenceTest<TEntity> IdentityKey<TProperty>(Expression<Func<TEntity, TProperty>> key)
      {
         return this.AddKeyProperty(new IdentityKeyProperty<TEntity, TProperty>(this.entityComparer, key));
      }

      public EntityPersistenceTest<TEntity> IdentityKey(Expression<Func<TEntity, long>> key)
      {
         return this.IdentityKey<long>(key);
      }

      public EntityPersistenceTest<TEntity> KeyProperty<TProperty>(Expression<Func<TEntity, TProperty>> property, TProperty value)
      {
         return this.AddKeyProperty(new KeyProperty<TEntity, TProperty>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> KeyReference<TReferenceEntity>(Expression<Func<TEntity, TReferenceEntity>> property, TReferenceEntity value)
         where TReferenceEntity : class
      {
         return this.AddKeyProperty(new KeyReferenceProperty<TEntity, TReferenceEntity>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> Property(Expression<Func<TEntity, string>> property, string value)
      {
         return this.AddProperty(new Property<TEntity, string>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> property, TProperty value)
         where TProperty : struct
      {
         return this.AddProperty(new Property<TEntity, TProperty>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty?>> property, TProperty? value)
         where TProperty : struct
      {
         return this.AddProperty(new Property<TEntity, TProperty?>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> ReadOnlyProperty(Expression<Func<TEntity, string>> property, string value)
      {
         return this.AddProperty(new Property<TEntity, string>(this.entityComparer, property, value, true));
      }

      public EntityPersistenceTest<TEntity> ReadOnlyProperty<TProperty>(Expression<Func<TEntity, TProperty>> property, TProperty value)
         where TProperty : struct
      {
         return this.AddProperty(new Property<TEntity, TProperty>(this.entityComparer, property, value, true));
      }

      public EntityPersistenceTest<TEntity> Reference<TReferenceEntity>(Expression<Func<TEntity, TReferenceEntity>> property, TReferenceEntity value)
         where TReferenceEntity : class
      {
         return this.AddProperty(new ReferenceProperty<TEntity, TReferenceEntity>(this.entityComparer, property, value));
      }

      public EntityPersistenceTest<TEntity> ReferenceList<TReferenceEntity, TList>(Expression<Func<TEntity, TList>> property, TList list)
         where TReferenceEntity : class
         where TList : IEnumerable<TReferenceEntity>
      {
         return this.AddProperty(new ListProperty<TEntity, TReferenceEntity, TList>(this.entityComparer, property, list));
      }

      public EntityPersistenceTest<TEntity> ReferenceList<TReferenceEntity>(Expression<Func<TEntity, IEnumerable<TReferenceEntity>>> property, Action<TEntity, TReferenceEntity> addMethod, TReferenceEntity item)
         where TReferenceEntity : class
      {
         return this.AddProperty(new ListProperty<TEntity, TReferenceEntity, IEnumerable<TReferenceEntity>>(this.entityComparer, property, addMethod, new List<TReferenceEntity> { item }));
      }

      public TEntity Create()
      {
         var entity = new TEntity();

         // Push the test values into the entity through the properties
         foreach (var property in this.properties.Values.Where(c => !c.IsReadOnly))
         {
            property.PushValue(entity);
         }

         this.unitOfWork.Add(entity);
         this.unitOfWork.Save();

         // Pull the identity value from the entity
         if (this.identityKeyProperty != null)
         {
            this.keyProperties[0].PullValue(entity);
         }

         return entity;
      }

      public TEntity AssertAll()
      {
         var newEntity = this.Create();

         // Assert that the identity value has changed from its default
         if (this.identityKeyProperty != null)
         {
            this.identityKeyProperty.AssertNonZeroID();
         }

         // Create a new unit of work to test the Create method
         using (var localUnitOfWork = this.unitOfWorkFactory.Create())
         {
            // Get the respective EntityReader
            IQueryable<TEntity> query = localUnitOfWork.Query<TEntity>();

            // Add a filter for each key
            foreach (var keyProperty in this.keyProperties)
            {
               query = query.Where(keyProperty.FilterExpression);
            }

            // Get the entity from the data-store using the key
            var entity = query.SingleOrDefault();

            Assert.IsNotNull(entity);

            // Assert that the test values have been returned through the properties
            // This is inside the UnitOfWork since the comparison methods may trigger lazy loading
            foreach (var property in this.properties.Values)
            {
               property.AssertAreEqual(entity);
            }
         }

         return newEntity;
      }

      private EntityPersistenceTest<TEntity> AddProperty(IProperty<TEntity> property)
      {
         this.properties.Add(property.Name, property);

         return this;
      }

      private EntityPersistenceTest<TEntity> AddKeyProperty(IKeyProperty<TEntity> property)
      {
         var newIdentityKeyProperty = property as IIdentityKeyProperty<TEntity>;

         if ((newIdentityKeyProperty != null) && (this.keyProperties.Count != 0))
         {
            throw new InvalidOperationException("Key properties already registered");
         }
         
         if ((newIdentityKeyProperty == null) && (this.identityKeyProperty != null))
         {
            throw new InvalidOperationException("IdentityKey property already registered");
         }

         this.properties.Add(property.Name, property);
         this.keyProperties.Add(property);

         this.identityKeyProperty = newIdentityKeyProperty;

         return this;
      }
   }
}
