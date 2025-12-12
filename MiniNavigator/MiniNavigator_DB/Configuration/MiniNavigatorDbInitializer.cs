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
    public class MiniNavigatorDbInitializer : DropCreateDatabaseAlways<MiniNavigatorDbContext>
    {
        protected override void Seed(MiniNavigatorDbContext db)
        {
            // Объект-тип "Роль"
            var roleTypeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = null,
                ObjectType = null,
                ParentID = null,
                Parent = null
            };
            db.BaseObjects.Add(roleTypeObject);

            var roleType = new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = roleTypeObject.ID,
                Base = roleTypeObject,
                Name = "Роль"
            };
            db.ObjectTypes.Add(roleType);
            db.SaveChanges();

            // Объект-тип "Пользователь"
            var userTypeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = null,
                ObjectType = null,
                ParentID = null,
                Parent = null
            };
            db.BaseObjects.Add(userTypeObject);

            var userType = new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = userTypeObject.ID,
                Base = userTypeObject,
                Name = "Пользователь"
               
            };
            db.ObjectTypes.Add(userType);
            db.SaveChanges();

            // Объект-тип "Действие"
            var actionTypeObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = null,
                ObjectType = null,
                ParentID = null,
                Parent = null
            };
            db.BaseObjects.Add(actionTypeObject);

            var actionType = new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = actionTypeObject.ID,
                Base = actionTypeObject,
                Name = "Действие"
            };
            db.ObjectTypes.Add(actionType);
            db.SaveChanges();

            // Объекты действий
            var actionAddObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = actionTypeObject.ID,
                ObjectType = actionTypeObject,
                ParentID = actionTypeObject.ID,
                Parent = actionTypeObject
            };
            db.BaseObjects.Add(actionAddObject);
            
            var actionEditObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = actionTypeObject.ID,
                ObjectType = actionTypeObject,
                ParentID = actionTypeObject.ID,
                Parent = actionTypeObject
            };
            db.BaseObjects.Add(actionEditObject);

            var actionDeleteObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = actionTypeObject.ID,
                ObjectType = actionTypeObject,
                ParentID = actionTypeObject.ID,
                Parent = actionTypeObject
            };
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

            // Роль А
            var roleAObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = roleTypeObject.ID,
                ObjectType = roleTypeObject,
                ParentID = roleTypeObject.ID,
                Parent = roleTypeObject
            };
            var roleA = new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleTypeObject.ID,
                Base = roleTypeObject
            };

            db.ObjectRoles.Add(roleA);
            db.BaseObjects.Add(roleAObject);

            // Пользователь А
            var userAObject = new BaseObject
            {
                ID = Guid.NewGuid(),
                ObjectTypeID = userTypeObject.ID,
                ObjectType = userTypeObject,
                ParentID = userTypeObject.ID,
                Parent = userTypeObject
            };
            var userA = new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userTypeObject.ID,
                Base = userTypeObject,
                RoleID = roleA.ID,
                Role = roleA
            };

            db.BaseObjects.Add(userAObject);
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
    }
}
