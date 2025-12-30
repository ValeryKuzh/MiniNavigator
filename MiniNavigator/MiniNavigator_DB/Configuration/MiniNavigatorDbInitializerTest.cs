using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_DB.Configuration
{
    internal class MiniNavigatorDbInitializerTest : DropCreateDatabaseAlways<MiniNavigatorDbContext>
    {
        protected override void Seed(MiniNavigatorDbContext db)
        {
            #region ObjectTypes

            BaseObject roleTypeObj = CreateBase(null);
            BaseObject userTypeObj = CreateBase(null);
            BaseObject productTypeObj = CreateBase(null);
            BaseObject attributeTypeObj = CreateBase(null);

            ObjectType roleType = CreateType(roleTypeObj, "Роль", true);
            ObjectType userType = CreateType(userTypeObj, "Пользователь", true);
            ObjectType productType = CreateType(productTypeObj, "Продукт", true);
            ObjectType attributeType = CreateType(attributeTypeObj, "Атрибут", false);

            db.BaseObjects.AddRange(new[]
            {
            roleTypeObj, userTypeObj, productTypeObj, attributeTypeObj
        });

            db.ObjectTypes.AddRange(new[]
            {
            roleType, userType, productType, attributeType
        });

            db.SaveChanges();

            #endregion

            #region Roles

            BaseObject adminRoleObj = CreateBase(roleTypeObj);
            BaseObject managerRoleObj = CreateBase(roleTypeObj);
            BaseObject userRoleObj = CreateBase(roleTypeObj);

            ObjectRole adminRole = CreateRole(adminRoleObj);
            ObjectRole managerRole = CreateRole(managerRoleObj);
            ObjectRole userRole = CreateRole(userRoleObj);

            db.BaseObjects.AddRange(new[]
            {
            adminRoleObj, managerRoleObj, userRoleObj
        });

            db.ObjectRoles.AddRange(new[]
            {
            adminRole, managerRole, userRole
        });

            db.SaveChanges();

            #endregion

            #region Users

            List<UserSeed> users = new List<UserSeed>
        {
            CreateUser(userTypeObj, adminRole),
            CreateUser(userTypeObj, managerRole),
            CreateUser(userTypeObj, userRole),
            CreateUser(userTypeObj, userRole)
        };

            db.BaseObjects.AddRange(users.Select(u => u.Base));
            db.ObjectUsers.AddRange(users.Select(u => u.User));
            db.SaveChanges();

            #endregion

            #region Products

            List<BaseObject> products = new List<BaseObject>
        {
            CreateBase(productTypeObj),
            CreateBase(productTypeObj),
            CreateBase(productTypeObj)
        };

            db.BaseObjects.AddRange(products);
            db.SaveChanges();

            #endregion

            #region Attributes

            List<ObjectAttribute> attributes = new List<ObjectAttribute>
        {
            CreateAttribute(attributeTypeObj, "Name", typeof(string)),
            CreateAttribute(attributeTypeObj, "Surname", typeof(string)),
            CreateAttribute(attributeTypeObj, "Email", typeof(string)),
            CreateAttribute(attributeTypeObj, "Phone", typeof(string)),
            CreateAttribute(attributeTypeObj, "Age", typeof(int)),
            CreateAttribute(attributeTypeObj, "IsActive", typeof(bool)),
            CreateAttribute(attributeTypeObj, "CreatedAt", typeof(DateTime)),
            CreateAttribute(attributeTypeObj, "LastLogin", typeof(DateTime)),
            CreateAttribute(attributeTypeObj, "Salary", typeof(decimal)),
            CreateAttribute(attributeTypeObj, "Price", typeof(decimal)),
            CreateAttribute(attributeTypeObj, "Description", typeof(string))
        };

            ObjectAttribute roleRefAttr = CreateReferenceAttribute(
                attributeTypeObj, "Role", roleType);

            attributes.Add(roleRefAttr);

            db.BaseObjects.AddRange(attributes.Select(a => a.Base));
            db.ObjectAttributes.AddRange(attributes);
            db.SaveChanges();

            #endregion

            #region ObjectTypeAttributes

            int order = 1;
            foreach (ObjectAttribute attr in attributes)
            {
                db.ObjectTypeAttributes.Add(new ObjectTypeAttribute
                {
                    ObjectTypeID = userType.ID,
                    AttributeID = attr.ID,
                    Order = order++,
                    IsVisible = true,
                    IsRequired = attr.Name == "Name" || attr.Name == "Email"
                });
            }

            db.ObjectTypeAttributes.AddRange(new[]
            {
            CreateOTA(productType, "Name", 1, attributes),
            CreateOTA(productType, "Price", 2, attributes),
            CreateOTA(productType, "Description", 3, attributes)
        });

            db.SaveChanges();

            #endregion

            #region AttributeValues

            foreach (UserSeed u in users)
            {
                AddValue(db, u.Base, "Name", "Test");
                AddValue(db, u.Base, "Surname", "User");
                AddValue(db, u.Base, "Email", "user@mail.com");
                AddValue(db, u.Base, "Age", "30");
                AddValue(db, u.Base, "IsActive", "true");
                AddValue(db, u.Base, "Role", adminRoleObj.ID.ToString());
            }

            foreach (BaseObject product in products)
            {
                AddValue(db, product, "Name", "Test product");
                AddValue(db, product, "Price", "199.99");
                AddValue(db, product, "Description", "Seeded product");
            }

            db.SaveChanges();

            #endregion

            base.Seed(db);
        }

        #region Helpers (совместимо с .NET 4.8)

        private BaseObject CreateBase(BaseObject type)
        {
            return new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectType = type,
                ObjectTypeID = type != null ? type.ID : (Guid?)null,
                Parent = type,
                ParentID = type != null ? type.ID : (Guid?)null
            };
        }

        private ObjectType CreateType(BaseObject baseObj, string name, bool visible)
        {
            return new ObjectType
            {
                ID = Guid.NewGuid(),
                Base = baseObj,
                Base_ID = baseObj.ID,
                Name = name,
                IsVisible = visible
            };
        }

        private ObjectRole CreateRole(BaseObject obj)
        {
            return new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base = obj,
                Base_ID = obj.ID
            };
        }

        private UserSeed CreateUser(BaseObject type, ObjectRole role)
        {
            BaseObject obj = CreateBase(type);

            ObjectUser user = new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base = obj,
                Base_ID = obj.ID,
                RoleID = role.ID,
                Role = role
            };

            return new UserSeed { Base = obj, User = user };
        }

        private ObjectAttribute CreateAttribute(
            BaseObject attrTypeObj, string name, Type valueType)
        {
            return new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = CreateBase(attrTypeObj),
                Name = name,
                ValueType = valueType.ToString()
            };
        }

        private ObjectAttribute CreateReferenceAttribute(
            BaseObject attrTypeObj, string name, ObjectType refType)
        {
            return new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = CreateBase(attrTypeObj),
                Name = name,
                IsReference = true,
                ReferenceObjectType = refType,
                ReferenceObjectTypeID = refType.ID,
                ValueType = typeof(Guid).ToString()
            };
        }

        private ObjectTypeAttribute CreateOTA(
            ObjectType type, string attrName, int order,
            List<ObjectAttribute> attrs)
        {
            ObjectAttribute attr = attrs.First(a => a.Name == attrName);

            return new ObjectTypeAttribute
            {
                ObjectTypeID = type.ID,
                AttributeID = attr.ID,
                Order = order,
                IsVisible = true
            };
        }

        private void AddValue(
            MiniNavigatorDbContext db,
            BaseObject obj,
            string attrName,
            string value)
        {
            ObjectAttribute attr = db.ObjectAttributes
                .First(a => a.Name == attrName);

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = obj,
                ObjectID = obj.ID,
                Attribute = attr,
                AttributeID = attr.ID,
                Value = value
            });
        }

        private class UserSeed
        {
            public BaseObject Base;
            public ObjectUser User;
        }

        #endregion
    }
}
