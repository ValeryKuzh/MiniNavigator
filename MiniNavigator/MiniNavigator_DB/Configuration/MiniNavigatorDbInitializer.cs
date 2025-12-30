using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MiniNavigator_DB.Configuration
{
    public class MiniNavigatorDbInitializer : DropCreateDatabaseAlways<MiniNavigatorDbContext>
    {
        protected override void Seed(MiniNavigatorDbContext db)
        {
            #region Object Types

            var actionTypeObject = CreateNewObject(null);
            var actionType = CreateNewObjectType(actionTypeObject, "Действие", false);

            var attributeTypeObject = CreateNewObject(null);
            var attributeType = CreateNewObjectType(attributeTypeObject, "Атрибут", false);

            var roleTypeObject = CreateNewObject(null);
            var roleType = CreateNewObjectType(roleTypeObject, "Роль", true);

            var userTypeObject = CreateNewObject(null);
            var userType = CreateNewObjectType(userTypeObject, "Пользователь", true);

            var fileTypeObject = CreateNewObject(null);
            var fileType = CreateNewObjectType(fileTypeObject, "Файл", true);

            var pdfTypeObject = CreateNewObject(fileTypeObject);
            var pdfType = CreateNewObjectType(pdfTypeObject, "PDF", true);

            var excelTypeObject = CreateNewObject(fileTypeObject);
            var excelType = CreateNewObjectType(excelTypeObject, "Excel", true);

            db.BaseObjects.AddRange(new[] { actionTypeObject, attributeTypeObject, roleTypeObject, userTypeObject, fileTypeObject, pdfTypeObject, excelTypeObject });
            db.ObjectTypes.AddRange(new[] { actionType, attributeType, roleType, userType, fileType, pdfType, excelType });
            db.SaveChanges();

            #endregion

            #region Actions
            var actionEditObject = CreateNewObject(actionTypeObject);

            db.BaseObjects.AddRange(new[] { actionEditObject });
            db.SaveChanges();

            db.ObjectActions.AddRange(new[]
            {
                new ObjectAction
                {
                    ID = Guid.NewGuid(),
                    Base_ID = actionEditObject.ID,
                    Base = actionEditObject,
                    Name = "EDIT",
                    DisplayName = "Редактировать",
                    ObjectTypes = new[] { roleType, userType }
                },
                new ObjectAction
                {
                    ID = Guid.NewGuid(),
                    Base_ID = actionEditObject.ID,
                    Base = actionEditObject,
                    Name = "DELETE",
                    DisplayName = "Удалить",
                    ObjectTypes = new[] { roleType, userType }
                }
            });
            db.SaveChanges();

            #endregion

            #region Roles and Users

            var roleAObject = CreateNewObject(roleTypeObject);
            var roleA = new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleAObject.ID,
                Base = roleAObject
            };

            var roleBObject = CreateNewObject(roleTypeObject);
            var roleB = new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleBObject.ID,
                Base = roleBObject
            };


            var userAObject = CreateNewObject(userTypeObject);
            var userA = new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userAObject.ID,
                Base = userAObject,
                RoleID = roleA.ID,
                Role = roleA
            };

            db.BaseObjects.AddRange(new[] { roleAObject, userAObject });
            db.ObjectRoles.Add(roleA);
            db.ObjectUsers.Add(userA);
            db.SaveChanges();

            #endregion

            #region Files

            var filePDFObject = CreateNewObject(pdfTypeObject);
            var filePDF = new ObjectFile
            {
                ID = Guid.NewGuid(),
                Base_ID = filePDFObject.ID,
                Base = filePDFObject,
                FileExtension = "PDF"
            };

            db.BaseObjects.Add(filePDFObject);
            db.ObjectFiles.Add(filePDF);
            db.SaveChanges();

            #endregion

            #region Attributes

            var nameAttrObject = CreateNewObject(attributeTypeObject);
            var surnameAttrObject = CreateNewObject(attributeTypeObject);
            var ageAttrObject = CreateNewObject(attributeTypeObject);
            var titleAttrObject = CreateNewObject(attributeTypeObject);
            var roleAttrObject = CreateNewObject(attributeTypeObject);
            var objectOwnerAttrObject = CreateNewObject(attributeTypeObject);

            var nameAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = nameAttrObject,
                Name = "Name",
                ValueType = typeof(string).ToString()
            };
            var surnameAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = surnameAttrObject,
                Name = "Surname",
                ValueType = typeof(string).ToString()
            };
            var ageAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = ageAttrObject,
                Name = "Age",
                ValueType = typeof(byte).ToString()
            };
            var titleAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = titleAttrObject,
                Name = "Title",
                ValueType = typeof(string).ToString()
            };
            var roleAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = roleAttrObject,
                Name = "Role",
                IsReference = true,
                ReferenceObjectType = roleType,
                ReferenceObjectTypeID = roleType.ID,
                ValueType = typeof(Guid).ToString()
            };

            var objectOwnerAttribute = new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = objectOwnerAttrObject,
                Name = "Object Owner",
                IsReference = true,
                ReferenceObjectType = userType,
                ReferenceObjectTypeID = userType.ID,
                ValueType = typeof(Guid).ToString()
            };

            db.BaseObjects.AddRange(new[] { nameAttrObject, surnameAttrObject, ageAttrObject, titleAttrObject, roleAttrObject, objectOwnerAttrObject });
            db.ObjectAttributes.AddRange(new[] { nameAttribute, surnameAttribute, ageAttribute, titleAttribute, roleAttribute, objectOwnerAttribute });
            db.SaveChanges();

            #endregion

            #region ObjectTypeAttributes

            db.ObjectTypeAttributes.AddRange(new[]
            {
                new ObjectTypeAttribute
                {
                    ObjectTypeID = userType.ID,
                    AttributeID = nameAttribute.ID,
                    IsRequired = true,
                    IsVisible = true,
                    Order = 1
                },
                new ObjectTypeAttribute
                {
                    ObjectTypeID = userType.ID,
                    AttributeID = surnameAttribute.ID,
                    IsRequired = true,
                    IsVisible = true,
                    Order = 2
                },
                new ObjectTypeAttribute
                {
                    ObjectTypeID = userType.ID,
                    AttributeID = roleAttribute.ID,
                    IsRequired = true,
                    IsVisible = true,
                    Order = 3
                },
                new ObjectTypeAttribute
                {
                    ObjectTypeID = userType.ID,
                    AttributeID = ageAttribute.ID,
                    IsRequired = false,
                    IsVisible = true,
                    Order = 4
                },
                new ObjectTypeAttribute
                {
                    ObjectTypeID = roleType.ID,
                    AttributeID = titleAttribute.ID,
                    IsRequired = true,
                    IsVisible = true,
                    Order = 1
                },
                new ObjectTypeAttribute
                {
                    ObjectTypeID = pdfType.ID,
                    AttributeID = objectOwnerAttribute.ID,
                    IsRequired = true,
                    IsVisible = true,
                    Order = 1
                },
            });
            db.SaveChanges();

            #endregion

            #region ObjectAttributeValues

            db.ObjectAttributeValues.AddRange(new[]
            {
                new ObjectAttributeValue
                {
                    Object = userAObject,
                    ObjectID = userAObject.ID,
                    Attribute = nameAttribute,
                    AttributeID = nameAttribute.ID,
                    Value = "Valery"
                },
                new ObjectAttributeValue
                {
                    Object = userAObject,
                    ObjectID = userAObject.ID,
                    Attribute = surnameAttribute,
                    AttributeID = surnameAttribute.ID,
                    Value = "Kuzhovnik"
                },
                new ObjectAttributeValue
                {
                    Object = userAObject,
                    ObjectID = userAObject.ID,
                    Attribute = ageAttribute,
                    AttributeID = ageAttribute.ID,
                    Value = "19"
                },
                new ObjectAttributeValue
                {
                    Object = roleAObject,
                    ObjectID = roleAObject.ID,
                    Attribute = titleAttribute,
                    AttributeID = titleAttribute.ID,
                    Value = "Admin"
                },
                new ObjectAttributeValue
                {
                    Object = roleBObject,
                    ObjectID = roleBObject.ID,
                    Attribute = titleAttribute,
                    AttributeID = titleAttribute.ID,
                    Value = "Manager"
                },
                new ObjectAttributeValue
                {
                    Object = userAObject,
                    ObjectID = userAObject.ID,
                    Attribute = roleAttribute,
                    AttributeID = roleAttribute.ID,
                    Value = roleAObject.ID.ToString()
                }
            });
            db.SaveChanges();
            #endregion

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