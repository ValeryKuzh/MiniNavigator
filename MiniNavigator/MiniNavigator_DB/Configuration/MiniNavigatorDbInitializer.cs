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
            var roleType = CreateNewObjectType(roleTypeObject, "Роль");
            db.ObjectTypes.Add(roleType);
            db.SaveChanges();

            // Объект-тип "Пользователь"
            var userTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(userTypeObject);
            //Тип "Пользователь"
            var userType = CreateNewObjectType(userTypeObject, "Пользователь");
            db.ObjectTypes.Add(userType);
            db.SaveChanges();

            // Объект-тип "Действие"
            var actionTypeObject = CreateNewObject(null);
            db.BaseObjects.Add(actionTypeObject);
            //Тип "Действие"
            var actionType = CreateNewObjectType(actionTypeObject, "Действие");
            db.ObjectTypes.Add(actionType);
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
                ObjectTypes = new[] { roleType, userType }
            });
            db.ObjectActions.Add(new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = actionDeleteObject.ID,
                Base = actionDeleteObject,
                Name = "Удалить",
                ObjectTypes = new[] { roleType, userType }
            });
            db.SaveChanges();

            // Роль А
            var roleAObject = CreateNewObject(roleTypeObject);
            db.BaseObjects.Add(roleAObject);
            var roleA = new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleTypeObject.ID,
                Base = roleTypeObject
            };
            db.ObjectRoles.Add(roleA);
            db.SaveChanges();

            // Пользователь А
            var userAObject = CreateNewObject(userTypeObject);
            db.BaseObjects.Add(userAObject);
            var userA = new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userTypeObject.ID,
                Base = userTypeObject,
                RoleID = roleA.ID,
                Role = roleA
            };
            db.ObjectUsers.Add(userA);
            db.SaveChanges();

            var NameAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = roleTypeObject.ID,
                ObjectType = roleTypeObject,
                ParentID = roleTypeObject.ID,
                Parent = roleTypeObject
            };
            var NameAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = NameAttributeObject,
                Name = "Name",
                ValueType = "string",
                ObjectTypes = new[] { roleType, userType }
            };

            var SurnameAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = roleTypeObject.ID,
                ObjectType = roleTypeObject,
                ParentID = roleTypeObject.ID,
                Parent = roleTypeObject
            };
            var SurnameAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = SurnameAttributeObject,
                Name = "Surname",
                ValueType = "string",
                ObjectTypes = new[] { userType }
            };

            var AgeAttributeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = roleTypeObject.ID,
                ObjectType = roleTypeObject,
                ParentID = roleTypeObject.ID,
                Parent = roleTypeObject
            };
            var AgeAttribute = new ObjectAttribute()
            {
                ID = Guid.NewGuid(),
                Base = AgeAttributeObject,
                Name = "Age",
                ValueType = "byte",
                ObjectTypes = new[] { userType }
            };

            db.BaseObjects.Add(NameAttributeObject);
            db.BaseObjects.Add(SurnameAttributeObject);
            db.BaseObjects.Add(AgeAttributeObject);
            db.ObjectAttributes.Add(NameAttribute);
            db.ObjectAttributes.Add(SurnameAttribute);
            db.ObjectAttributes.Add(AgeAttribute);
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
        private ObjectType CreateNewObjectType(BaseObject objOfType, string name)
        {
            return new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = objOfType.ID,
                Base = objOfType,
                Name = name
            };
        }

    }
}
