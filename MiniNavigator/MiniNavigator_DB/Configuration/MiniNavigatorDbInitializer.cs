using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;

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

            var pdfFileTypeObject = CreateNewObject(fileTypeObject);
            var pdfFileType = CreateNewObjectType(pdfFileTypeObject, "PDF", true);

            var excelTypeObject = CreateNewObject(fileTypeObject);
            var excelFileType = CreateNewObjectType(excelTypeObject, "Excel", true);

            db.BaseObjects.AddRange(new[] { actionTypeObject, attributeTypeObject, roleTypeObject, userTypeObject, fileTypeObject, pdfFileTypeObject, excelTypeObject });
            db.ObjectTypes.AddRange(new[] { actionType, attributeType, roleType, userType, fileType, pdfFileType, excelFileType });
            db.SaveChanges();

            #endregion

            #region Actions

            var actionEditObject = CreateNewObject(actionTypeObject);
            var actionDeleteObject = CreateNewObject(actionTypeObject);

            db.BaseObjects.AddRange(new[] { actionEditObject });
            db.SaveChanges();

            db.ObjectActions.AddRange(new[]
            {
                CreateAction(actionEditObject, "EDIT", "Редактировать", roleType, userType, pdfFileType), 
                CreateAction(actionDeleteObject, "DELETE", "Удалить", roleType, userType, pdfFileType),
            });
            db.SaveChanges();

            #endregion

            #region Roles and Users

            var roleAObject = CreateNewObject(roleTypeObject);
            var roleA = CreateRole(roleAObject);    

            var roleBObject = CreateNewObject(roleTypeObject);
            var roleB = CreateRole(roleBObject);

            var userAObject = CreateNewObject(userTypeObject);
            var userA = CreateUser(userAObject, roleA);

            db.BaseObjects.AddRange(new[] { roleAObject, roleBObject, userAObject });

            db.ObjectRoles.AddRange(new[] { roleA, roleB });

            db.ObjectUsers.Add(userA);

            db.SaveChanges();

            #endregion

            #region Files

            var filePDFObject = CreateNewObject(pdfFileTypeObject);
            var filePDF = CreateFile(filePDFObject, "PDF");
            
            var fileExcelObject = CreateNewObject(pdfFileTypeObject);
            var fileExcel = CreateFile(fileExcelObject, "Excel");

            db.BaseObjects.AddRange(new[] { filePDFObject, fileExcelObject });

            db.ObjectFiles.AddRange(new[]{ filePDF, fileExcel });

            db.SaveChanges();

            #endregion

            #region Attributes

            var nameAttrObject = CreateNewObject(attributeTypeObject);
            var surnameAttrObject = CreateNewObject(attributeTypeObject);
            var ageAttrObject = CreateNewObject(attributeTypeObject);
            var titleAttrObject = CreateNewObject(attributeTypeObject);
            var roleAttrObject = CreateNewObject(attributeTypeObject);
            var objectOwnerAttrObject = CreateNewObject(attributeTypeObject);
            var fileNameAttrObject = CreateNewObject(attributeTypeObject);
            var createdAtAttrObject = CreateNewObject(attributeTypeObject);

            var nameAttribute = CreateAttribute("Name", nameAttrObject, false, null, typeof(string));
            var surnameAttribute = CreateAttribute("Surname", surnameAttrObject, false, null, typeof(string));
            var ageAttribute = CreateAttribute("Age", ageAttrObject, false, null, typeof(byte));
            var titleAttribute = CreateAttribute("Title", titleAttrObject, false, null, typeof(string));
            var roleAttribute = CreateAttribute("Role", roleAttrObject, true, roleType, typeof(Guid));
            var objectOwnerAttribute = CreateAttribute("Object Owner", objectOwnerAttrObject, true, userType, typeof(Guid));
            var fileNameAttribute = CreateAttribute("File Name", fileNameAttrObject, false, null, typeof(string));
            var createdAtAttribute = CreateAttribute("Created At", createdAtAttrObject, false, null, typeof(DateTime));


            db.BaseObjects.AddRange(new[] { 
                nameAttrObject, 
                surnameAttrObject, 
                ageAttrObject, 
                titleAttrObject, 
                roleAttrObject, 
                objectOwnerAttrObject,
                fileNameAttrObject,
                createdAtAttrObject
            });
            db.ObjectAttributes.AddRange(new[] { 
                nameAttribute, 
                surnameAttribute, 
                ageAttribute, 
                titleAttribute,
                roleAttribute, 
                objectOwnerAttribute,
                fileNameAttribute,
                createdAtAttribute 
            });
            db.SaveChanges();

            #endregion

            #region ObjectTypeAttributes

            db.ObjectTypeAttributes.AddRange(new[]
            {
                CreateObjectTypeAttribute(userType, nameAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(userType, surnameAttribute, true, true, true, 2),
                CreateObjectTypeAttribute(userType, roleAttribute, true, true, false, 3),
                CreateObjectTypeAttribute(userType, ageAttribute, true, true, false, 4),

                CreateObjectTypeAttribute(roleType, titleAttribute, true, true, true, 1),

                CreateObjectTypeAttribute(pdfFileType, fileNameAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(pdfFileType, objectOwnerAttribute, true, true, false, 2),
                CreateObjectTypeAttribute(pdfFileType, createdAtAttribute, false, true, false, 3),

                CreateObjectTypeAttribute(excelFileType, fileNameAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(excelFileType, objectOwnerAttribute, true, true, false, 2),
                CreateObjectTypeAttribute(excelFileType, createdAtAttribute, false, true, false, 3)
            });
            db.SaveChanges();

            #endregion

            #region ObjectAttributeValues

            db.ObjectAttributeValues.AddRange(new[]
            {
                CreateAttributeValue(roleAObject, titleAttribute, "Admin"),

                CreateAttributeValue(roleBObject, titleAttribute, "Manager"),

                CreateAttributeValue(userAObject, nameAttribute, "Valery"),
                CreateAttributeValue(userAObject, surnameAttribute, "Kuzhovnik"),
                CreateAttributeValue(userAObject, roleAttribute, roleAObject.ID.ToString()),
                CreateAttributeValue(userAObject, ageAttribute, "19"),

                CreateAttributeValue(filePDFObject, fileNameAttribute, "lol.pdf"),
                CreateAttributeValue(filePDFObject, objectOwnerAttribute, userAObject.ID.ToString()),
                CreateAttributeValue(filePDFObject, createdAtAttribute, DateTime.Now.ToString()),

                CreateAttributeValue(fileExcelObject, fileNameAttribute, "lol.xlsx"),
                CreateAttributeValue(fileExcelObject, objectOwnerAttribute, userAObject.ID.ToString()),
                CreateAttributeValue(fileExcelObject, createdAtAttribute, DateTime.Now.ToString())
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

        private ObjectRole CreateRole(BaseObject roleObject)
        {
            return new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleObject.ID,
                Base = roleObject
            };
        }

        private ObjectUser CreateUser(BaseObject userObject, ObjectRole role)
        {
            return new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userObject.ID,
                Base = userObject,
                RoleID = role?.ID,
                Role = role
            };
        }

        private ObjectFile CreateFile(BaseObject fileObject, string fileExtension)
        {
            return new ObjectFile
            {
                ID = Guid.NewGuid(),
                Base_ID = fileObject.ID,
                Base = fileObject,
                FileExtension = fileExtension
            };
        }

        private ObjectAttribute CreateAttribute(string attributeName, BaseObject attributeObject, bool isReference, ObjectType referenceObjectType, Type type)
        {
            return new ObjectAttribute
            {
                ID = Guid.NewGuid(),
                Base = attributeObject,
                Name = attributeName,
                IsReference = isReference,
                ReferenceObjectType = referenceObjectType,
                ReferenceObjectTypeID = referenceObjectType?.ID,
                ValueType = type.ToString() 
            };
        }

        private ObjectAction CreateAction(BaseObject baseObject, string commandName, string displayName, params ObjectType[] types)
        {
            return new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = baseObject.ID,
                Base = baseObject,
                Name = commandName,
                DisplayName = displayName,
                ObjectTypes = types
            };
        }

        private ObjectTypeAttribute CreateObjectTypeAttribute(ObjectType objectType, ObjectAttribute attribute, bool isRequired, bool isVisible, bool isTitle, int index)
        {
            return new ObjectTypeAttribute
            {
                ObjectTypeID = objectType.ID,
                AttributeID = attribute.ID,
                IsRequired = isRequired,
                IsVisible = isVisible,
                IsTitle = isTitle,
                Order = index
            };
        }

        private ObjectAttributeValue CreateAttributeValue(BaseObject baseObject, ObjectAttribute attribute, string value)
        {
            return new ObjectAttributeValue
            {
                Object = baseObject,
                ObjectID = baseObject.ID,
                Attribute = attribute,
                AttributeID = attribute.ID,
                Value = value
            };
        }
    }
}