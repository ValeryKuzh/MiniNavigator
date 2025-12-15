using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniNavigator_DB.Configuration
{
    public class MiniNavigatorDbInitializer : DropCreateDatabaseAlways<MiniNavigatorDbContext>
    {
        private readonly MiniNavigatorDbContext _db;
        public MiniNavigatorDbInitializer(MiniNavigatorDbContext db)
        {
            _db = db;
        }
        protected override void Seed(MiniNavigatorDbContext db)
        {
            // Объект-тип "Роль"
            var roleTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(roleTypeObject);
            //Тип "Роль"
            var roleType = CreateNewObjectType(roleTypeObject, "Роль", true);
            db.ObjectTypes.Add(roleType);
            db.SaveChanges();

            // Объект-тип "Пользователь"
            var userTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(userTypeObject);
            //Тип "Пользователь"
            var userType = CreateNewObjectType(userTypeObject, "Пользователь", true);
            db.ObjectTypes.Add(userType);
            db.SaveChanges();

            // Объект-тип "Действие"
            var actionTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(actionTypeObject);
            //Тип "Действие"
            var actionType = CreateNewObjectType(actionTypeObject, "Действие", false);
            db.ObjectTypes.Add(actionType);
            db.SaveChanges();

            // Объект-тип "Атрибут"
            var attributeTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(attributeTypeObject);
            //Тип "Атрибут"
            var attributeType = CreateNewObjectType(attributeTypeObject, "Атрибут", false);
            db.ObjectTypes.Add(attributeType);
            db.SaveChanges();

            // Объекты действий

            // Объект действия "Добавить"
            var actionAddObject = CreateNewObject(actionTypeObject);
            db.BaseObjects.Add(actionAddObject);

            // Объект действия "Редактировать"
            var actionEditObject = CreateNewObject(actionTypeObject);
            db.BaseObjects.Add(actionEditObject);

            // Объект действия "Удалить"
            var actionDeleteObject = CreateNewObject(actionTypeObject);
            db.BaseObjects.Add(actionDeleteObject);

            db.SaveChanges();

            // ObjectActions, связанные с BaseObjects
            db.ObjectActions.Add(new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = actionAddObject.ID,
                Base = actionAddObject,
                Name = "Добавить",
                ObjectTypes = new[] { roleType, userType }
            });
            db.ObjectActions.Add(new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = actionEditObject.ID,
                Base = actionEditObject,
                Name = "Редактировать",
                ObjectTypes = new[] { roleType }
            });
            db.ObjectActions.Add(new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = actionDeleteObject.ID,
                Base = actionDeleteObject,
                Name = "Удалить",
                ObjectTypes = new[] { roleType }
            });
            db.SaveChanges();

            // Роль А
            var roleAObject = CreateNewObject(roleTypeObject);
            db.BaseObjects.Add(roleAObject);
            var roleA = new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleAObject.ID,
                Base = roleAObject
            };
            db.ObjectRoles.Add(roleA);
            db.SaveChanges();

            // Пользователь А
            var userAObject = CreateNewObject(userTypeObject);
            db.BaseObjects.Add(userAObject);
            var userA = new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userAObject.ID,
                Base = userAObject,
                RoleID = roleA.ID,
                Role = roleA
            };
            db.ObjectUsers.Add(userA);
            db.SaveChanges();

            var NameAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ObjectType = attributeTypeObject,
                ParentID = attributeTypeObject.ID,
                Parent = attributeTypeObject
            };
            var NameAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = NameAttributeObject,
                Name = "Name",
                ValueType = typeof(string).ToString(),
                ObjectTypes = new[] { roleType, userType }
            };

            var SurnameAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ObjectType = attributeTypeObject,
                ParentID = attributeTypeObject.ID,
                Parent = attributeTypeObject
            };
            var SurnameAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = SurnameAttributeObject,
                Name = "Surname",
                ValueType = typeof(string).ToString(),
                ObjectTypes = new[] { userType }
            };

            var AgeAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ObjectType = attributeTypeObject,
                ParentID = attributeTypeObject.ID,
                Parent = attributeTypeObject
            };
            var AgeAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = AgeAttributeObject,
                Name = "Age",
                ValueType = typeof(byte).ToString(),
                ObjectTypes = new[] { userType }
            };


            var TitleAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ObjectType = attributeTypeObject,
                ParentID = attributeTypeObject.ID,
                Parent = attributeTypeObject
            };
            var TitleAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = TitleAttributeObject,
                Name = "Title",
                ValueType = typeof(string).ToString(),
                ObjectTypes = new[] { roleType }
            };

            var RoleAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = attributeTypeObject.ID,
                ObjectType = attributeTypeObject,
                ParentID = attributeTypeObject.ID,
                Parent = attributeTypeObject
            };
            var RoleAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = RoleAttributeObject,
                Name = "Role",
                ValueType = typeof(string).ToString(),
                ObjectTypes = new[] { userType }
            };

            db.BaseObjects.Add(NameAttributeObject);
            db.BaseObjects.Add(SurnameAttributeObject);
            db.BaseObjects.Add(AgeAttributeObject);
            db.BaseObjects.Add(TitleAttributeObject);
            db.BaseObjects.Add(RoleAttributeObject);
            db.ObjectAttributes.Add(NameAttribute);
            db.ObjectAttributes.Add(SurnameAttribute);
            db.ObjectAttributes.Add(AgeAttribute);
            db.ObjectAttributes.Add(TitleAttribute);
            db.ObjectAttributes.Add(RoleAttribute);
            db.SaveChanges();

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = userAObject,
                ObjectID = userAObject.ID,
                Attribute = NameAttribute,
                AttributeID = NameAttribute.ID,
                Value = "Valery"
            });

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = userAObject,
                ObjectID = userAObject.ID,
                Attribute = SurnameAttribute,
                AttributeID = SurnameAttribute.ID,
                Value = "Kuzhovnik"
            });

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = userAObject,
                ObjectID = userAObject.ID,
                Attribute = AgeAttribute,
                AttributeID = AgeAttribute.ID,
                Value = "19"
            });

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = roleAObject,
                ObjectID = roleAObject.ID,
                Attribute = TitleAttribute,
                AttributeID = TitleAttribute.ID,
                Value = "Admin"
            });

            db.ObjectAttributeValues.Add(new ObjectAttributeValue
            {
                Object = userAObject,
                ObjectID = userAObject.ID,
                Attribute = RoleAttribute,
                AttributeID = RoleAttribute.ID,
                Value = "Admin"
            });

            db.SaveChanges();

            base.Seed(db);
        }

        private BaseObject CreateNewObject(BaseObject objType)
        {
            return new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = objType?.ID,
                ObjectType = objType,
                ParentID = objType?.ID,
                Parent = objType
            };
        }
        private ObjectType CreateNewObjectType(BaseObject objOfType, string name, bool isVisible)
        {
            return new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = objOfType.ID,
                Base = objOfType,
                Name = name,
                IsVisible = isVisible
            };
        }

    }
}
