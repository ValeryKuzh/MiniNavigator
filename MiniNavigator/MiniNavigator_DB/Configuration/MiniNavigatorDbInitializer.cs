using MiniNavigator_DB.Context;
using MiniNavigator_DB.Model;
using System;
using System.Data.Entity;

namespace MiniNavigator_DB.Configuration
{
    /// <summary>
    /// Класс-инициализатор для заполнения базы данных начальными значениями
    /// </summary>
    public class MiniNavigatorDbInitializer : DropCreateDatabaseAlways<MiniNavigatorDbContext>
    {
        /// <summary>
        /// Метод для инициализации базы данных начальными значениями
        /// </summary>
        /// <param name="db">Контекст базы данных</param>
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

            var excelFileTypeObject = CreateNewObject(fileTypeObject);
            var excelFileType = CreateNewObjectType(excelFileTypeObject, "Excel", true);

            var materialTypeObject = CreateNewObject(null);
            var materialsType = CreateNewObjectType(materialTypeObject, "Материал", true);

            var liquidTypeObject = CreateNewObject(materialTypeObject);
            var liquidType = CreateNewObjectType(liquidTypeObject, "Жидкость", true);

            var metallTypeObject = CreateNewObject(materialTypeObject);
            var metallType = CreateNewObjectType(metallTypeObject, "Металл", true);

            var partTypeObject = CreateNewObject(null);
            var partType = CreateNewObjectType(partTypeObject, "Деталь", true);

            db.BaseObjects.AddRange(new[] 
            {
                actionTypeObject, 
                attributeTypeObject, 
                roleTypeObject, 
                userTypeObject, 
                fileTypeObject, 
                pdfFileTypeObject, 
                excelFileTypeObject,
                materialTypeObject,
                liquidTypeObject,
                metallTypeObject
            });
            db.ObjectTypes.AddRange(new[] 
            { 
                actionType, 
                attributeType, 
                roleType, 
                userType, 
                fileType, 
                pdfFileType, 
                excelFileType,
                materialsType,
                liquidType,
                metallType
            });
            db.SaveChanges();

            #endregion

            #region Actions

            var actionEditObject = CreateNewObject(actionTypeObject);
            var actionDeleteObject = CreateNewObject(actionTypeObject);
            var actionDownloadObject = CreateNewObject(actionTypeObject);
            var actionOpenObject = CreateNewObject(actionTypeObject);

            db.BaseObjects.AddRange(new[] { actionEditObject, actionDeleteObject, actionDownloadObject, actionOpenObject });
            db.SaveChanges();

            db.ObjectActions.AddRange(new[]
            {
                CreateAction(actionEditObject, "EDIT", "Редактировать", roleType, userType, pdfFileType, excelFileType, metallType, liquidType, partType), 
                CreateAction(actionDeleteObject, "DELETE", "Удалить", roleType, userType, pdfFileType, excelFileType, metallType, liquidType, partType),
                CreateAction(actionDownloadObject, "DOWNLOAD", "Скачать", pdfFileType, excelFileType),
                CreateAction(actionOpenObject, "OPEN", "Открыть", pdfFileType, excelFileType),
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

            var userBObject = CreateNewObject(userTypeObject);
            var userB = CreateUser(userBObject, roleB);

            db.BaseObjects.AddRange(new[] { roleAObject, roleBObject, userAObject, userBObject });

            db.ObjectRoles.AddRange(new[] { roleA, roleB });

            db.ObjectUsers.AddRange(new[] { userA, userB });

            db.SaveChanges();

            #endregion

            #region Files

            var filePDFObject = CreateNewObject(pdfFileTypeObject);
            var filePDF = CreateFile(filePDFObject, "PDF");
            
            var fileExcelObject = CreateNewObject(excelFileTypeObject);
            var fileExcel = CreateFile(fileExcelObject, "Excel");

            db.BaseObjects.AddRange(new[] { filePDFObject, fileExcelObject });

            db.ObjectFiles.AddRange(new[]{ filePDF, fileExcel });

            db.SaveChanges();

            #endregion

            #region Metalls and Liquids

            var waterLiquid = CreateNewObject(liquidTypeObject);
            var milkLiquid = CreateNewObject(liquidTypeObject);
            var oilLiquid = CreateNewObject(liquidTypeObject);
            var alcoholLiquid = CreateNewObject(liquidTypeObject);
            var glycerinLiquid = CreateNewObject(liquidTypeObject);

            var ironMetall = CreateNewObject(metallTypeObject);
            var aluminumMetall = CreateNewObject(metallTypeObject);
            var cuprumMetall = CreateNewObject(metallTypeObject);
            var aurumMetall = CreateNewObject(metallTypeObject);
            var argentumMetall = CreateNewObject(metallTypeObject);

            db.BaseObjects.AddRange(new[] 
            {
                waterLiquid,
                milkLiquid, 
                oilLiquid,
                alcoholLiquid,
                glycerinLiquid,
                
                ironMetall,
                aluminumMetall,
                cuprumMetall,
                aurumMetall,
                argentumMetall
            });

            #endregion

            #region Parts

            var gearPart = CreateNewObject(partTypeObject);
            var shaftPart = CreateNewObject(partTypeObject);
            var bodyPart = CreateNewObject(partTypeObject);

            db.BaseObjects.AddRange(new[]
            {
                gearPart,
                shaftPart,
                bodyPart
            });

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
            var densityAttrObject = CreateNewObject(attributeTypeObject);
            var boilingPointAttrObject = CreateNewObject(attributeTypeObject);
            var freezingPointAttrObject = CreateNewObject(attributeTypeObject);
            var meltingPointAttrObject = CreateNewObject(attributeTypeObject);
            var thermalConductivityAttrObject = CreateNewObject(attributeTypeObject);
            var electricalConductivityAttrObject = CreateNewObject(attributeTypeObject);
            var metallMaterialAttrObject = CreateNewObject(attributeTypeObject);

            var nameAttribute = CreateAttribute("Name", nameAttrObject, false, null, typeof(string));
            var surnameAttribute = CreateAttribute("Surname", surnameAttrObject, false, null, typeof(string));
            var ageAttribute = CreateAttribute("Age", ageAttrObject, false, null, typeof(byte));
            var titleAttribute = CreateAttribute("Title", titleAttrObject, false, null, typeof(string));
            var roleAttribute = CreateAttribute("Role", roleAttrObject, true, roleType, typeof(Guid));
            var objectOwnerAttribute = CreateAttribute("Object Owner", objectOwnerAttrObject, true, userType, typeof(Guid));
            var fileNameAttribute = CreateAttribute("File Name", fileNameAttrObject, false, null, typeof(string));
            var createdAtAttribute = CreateAttribute("Created At", createdAtAttrObject, false, null, typeof(DateTime));
            var densityAttribute = CreateAttribute("Density", densityAttrObject, false, null, typeof(decimal));
            var boilingPointAttribute = CreateAttribute("Boiling Point", boilingPointAttrObject, false, null, typeof(decimal));
            var freezingPointAttribute = CreateAttribute("Freezing Point", freezingPointAttrObject, false, null, typeof(decimal));
            var meltingPointAttribute = CreateAttribute("Melting Point", meltingPointAttrObject, false, null, typeof(decimal));
            var thermalConductivityAttribute = CreateAttribute("Thermal Conductivity", thermalConductivityAttrObject, false, null, typeof(bool));
            var electricalConductivityAttribute = CreateAttribute("Electrical Conductivity", electricalConductivityAttrObject, false, null, typeof(bool));
            var metallMaterialAttribute = CreateAttribute("Metall", metallMaterialAttrObject, true, metallType, typeof(Guid));

            db.BaseObjects.AddRange(new[] { 
                nameAttrObject, 
                surnameAttrObject, 
                ageAttrObject, 
                titleAttrObject, 
                roleAttrObject, 
                objectOwnerAttrObject,
                fileNameAttrObject,
                createdAtAttrObject,
                densityAttrObject,
                boilingPointAttrObject,
                freezingPointAttrObject,
                meltingPointAttrObject,
                thermalConductivityAttrObject,
                electricalConductivityAttrObject,
                metallMaterialAttrObject
            });
            db.ObjectAttributes.AddRange(new[] { 
                nameAttribute, 
                surnameAttribute, 
                ageAttribute, 
                titleAttribute,
                roleAttribute, 
                objectOwnerAttribute,
                fileNameAttribute,
                createdAtAttribute,
                densityAttribute,
                boilingPointAttribute,
                freezingPointAttribute,
                meltingPointAttribute,
                thermalConductivityAttribute,
                electricalConductivityAttribute,
                metallMaterialAttribute
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
                CreateObjectTypeAttribute(excelFileType, createdAtAttribute, false, true, false, 3),

                CreateObjectTypeAttribute(liquidType, titleAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(liquidType, densityAttribute, true, true, false, 2),
                CreateObjectTypeAttribute(liquidType, freezingPointAttribute, true, true, false, 3),
                CreateObjectTypeAttribute(liquidType, boilingPointAttribute, true, true, false, 4),
                CreateObjectTypeAttribute(liquidType, meltingPointAttribute, true, true, false, 5),


                CreateObjectTypeAttribute(metallType, titleAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(metallType, densityAttribute, true, true, false, 2),
                CreateObjectTypeAttribute(metallType, thermalConductivityAttribute, true, true, false, 3),
                CreateObjectTypeAttribute(metallType, electricalConductivityAttribute, true, true, false, 4),

                CreateObjectTypeAttribute(partType, titleAttribute, true, true, true, 1),
                CreateObjectTypeAttribute(partType, metallMaterialAttribute, false, true, false, 2),
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


                CreateAttributeValue(userBObject, nameAttribute, "Alex"),
                CreateAttributeValue(userBObject, surnameAttribute, "Prikhodko"),
                CreateAttributeValue(userBObject, roleAttribute, roleAObject.ID.ToString()),
                CreateAttributeValue(userBObject, ageAttribute, "27"),

                CreateAttributeValue(filePDFObject, fileNameAttribute, "lol.pdf"),
                CreateAttributeValue(filePDFObject, objectOwnerAttribute, userAObject.ID.ToString()),
                CreateAttributeValue(filePDFObject, createdAtAttribute, DateTime.Now.ToString()),

                CreateAttributeValue(fileExcelObject, fileNameAttribute, "lol.xlsx"),
                CreateAttributeValue(fileExcelObject, objectOwnerAttribute, userAObject.ID.ToString()),
                CreateAttributeValue(fileExcelObject, createdAtAttribute, DateTime.Now.ToString()),
                
                // Liquid
                CreateAttributeValue(waterLiquid, titleAttribute, "Вода"),
                CreateAttributeValue(waterLiquid, densityAttribute, "1,0"),
                CreateAttributeValue(waterLiquid, freezingPointAttribute, "0,0"),
                CreateAttributeValue(waterLiquid, boilingPointAttribute, "0,0"),
                CreateAttributeValue(waterLiquid, meltingPointAttribute, "100,0"),

                CreateAttributeValue(milkLiquid, titleAttribute, "Молоко"),
                CreateAttributeValue(milkLiquid, densityAttribute, "1,03"),
                CreateAttributeValue(milkLiquid, freezingPointAttribute, "-0,5"),
                CreateAttributeValue(milkLiquid, boilingPointAttribute, "-0,5"),
                CreateAttributeValue(milkLiquid, meltingPointAttribute, "100,0"),

                CreateAttributeValue(oilLiquid, titleAttribute, "Растительное масло"),
                CreateAttributeValue(oilLiquid, densityAttribute, "0,92"),
                CreateAttributeValue(oilLiquid, freezingPointAttribute, "-10,0"),
                CreateAttributeValue(oilLiquid, boilingPointAttribute, "-10,0"),
                CreateAttributeValue(oilLiquid, meltingPointAttribute, "220,0"),

                CreateAttributeValue(alcoholLiquid, titleAttribute, "Спирт"),
                CreateAttributeValue(alcoholLiquid, densityAttribute, "0,79"),
                CreateAttributeValue(alcoholLiquid, freezingPointAttribute, "-114,0"),
                CreateAttributeValue(alcoholLiquid, boilingPointAttribute, "-114,0"),
                CreateAttributeValue(alcoholLiquid, meltingPointAttribute, "78,0"),

                CreateAttributeValue(glycerinLiquid, titleAttribute, "Глицерин"),
                CreateAttributeValue(glycerinLiquid, densityAttribute, "1,26"),
                CreateAttributeValue(glycerinLiquid, freezingPointAttribute, "17,8"),
                CreateAttributeValue(glycerinLiquid, boilingPointAttribute, "17,8"),
                CreateAttributeValue(glycerinLiquid, meltingPointAttribute, "290,0"),

                // Metall
                CreateAttributeValue(ironMetall, titleAttribute, "Железо"),
                CreateAttributeValue(ironMetall, densityAttribute, "7,87"),
                CreateAttributeValue(ironMetall, thermalConductivityAttribute, "true"),
                CreateAttributeValue(ironMetall, electricalConductivityAttribute, "true"),
                CreateAttributeValue(ironMetall, meltingPointAttribute, "1538"),

                CreateAttributeValue(aluminumMetall, titleAttribute, "Алюминий"),
                CreateAttributeValue(aluminumMetall, densityAttribute, "2,70"),
                CreateAttributeValue(aluminumMetall, thermalConductivityAttribute, "true"),
                CreateAttributeValue(aluminumMetall, electricalConductivityAttribute, "true"),
                CreateAttributeValue(aluminumMetall, meltingPointAttribute, "660"),
    
                CreateAttributeValue(cuprumMetall, titleAttribute, "Медь"),
                CreateAttributeValue(cuprumMetall, densityAttribute, "8,96"),
                CreateAttributeValue(cuprumMetall, thermalConductivityAttribute, "true"),
                CreateAttributeValue(cuprumMetall, electricalConductivityAttribute, "true"),
                CreateAttributeValue(cuprumMetall, meltingPointAttribute, "1085"),

                CreateAttributeValue(aurumMetall, titleAttribute, "Золото"),
                CreateAttributeValue(aurumMetall, densityAttribute, "19,32"),
                CreateAttributeValue(aurumMetall, thermalConductivityAttribute, "true"),
                CreateAttributeValue(aurumMetall, electricalConductivityAttribute, "true"),
                CreateAttributeValue(aurumMetall, meltingPointAttribute, "1064"),

                CreateAttributeValue(argentumMetall, titleAttribute, "Серебро"),
                CreateAttributeValue(argentumMetall, densityAttribute, "10,49"),
                CreateAttributeValue(argentumMetall, thermalConductivityAttribute, "true"),
                CreateAttributeValue(argentumMetall, electricalConductivityAttribute, "true"),
                CreateAttributeValue(argentumMetall, meltingPointAttribute, "962"),

                // Parts
                
                CreateAttributeValue(gearPart, titleAttribute, "Шестерня"),
                CreateAttributeValue(gearPart, metallMaterialAttribute, aluminumMetall.ID.ToString()),

                CreateAttributeValue(shaftPart, titleAttribute, "Вал"),
                CreateAttributeValue(shaftPart, metallMaterialAttribute, ironMetall.ID.ToString()),

                CreateAttributeValue(bodyPart, titleAttribute, "Корпус"),
                CreateAttributeValue(bodyPart, metallMaterialAttribute, aurumMetall.ID.ToString()),
            });
            db.SaveChanges();
            #endregion

            base.Seed(db);
        }

        /// <summary>
        /// Создаёт новый объект системы
        /// </summary>
        /// <param name="objType">Родитель объекта</param>
        /// <returns>Новый объект системы</returns>
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

        /// <summary>
        /// Создает тип объектов системы
        /// </summary>
        /// <param name="objOfType">Объект типа</param>
        /// <param name="name">Имя типа</param>
        /// <param name="isVisible">Отображается ли тип</param>
        /// <returns>Новый тип системы</returns>
        private ObjectType CreateNewObjectType(BaseObject objOfType, string name, bool isVisible)
        {
            if (objOfType == null) throw new ArgumentNullException("objOfType не может быть null");
            return new ObjectType
            {
                ID = Guid.NewGuid(),
                Base_ID = objOfType.ID,
                Base = objOfType,
                Name = name,
                IsVisible = isVisible
            };
        }

        /// <summary>
        /// Создает новую роль
        /// </summary>
        /// <param name="roleObject">Объект роли</param>
        /// <returns>Новая роль</returns>
        private ObjectRole CreateRole(BaseObject roleObject)
        {
            if (roleObject == null) throw new ArgumentNullException("roleObject не может быть null");
            return new ObjectRole
            {
                ID = Guid.NewGuid(),
                Base_ID = roleObject.ID,
                Base = roleObject
            };
        }

        /// <summary>
        /// Создаёт нового пользователя
        /// </summary>
        /// <param name="userObject">Объект пользователя</param>
        /// <param name="role">Роль</param>
        /// <returns>Новый пользователь</returns>
        private ObjectUser CreateUser(BaseObject userObject, ObjectRole role)
        {
            if (userObject == null) throw new ArgumentNullException("userObject не может быть null");
            return new ObjectUser
            {
                ID = Guid.NewGuid(),
                Base_ID = userObject.ID,
                Base = userObject,
                RoleID = role?.ID,
                Role = role
            };
        }

        /// <summary>
        /// Создает новый файл
        /// </summary>
        /// <param name="fileObject">Объект файла</param>
        /// <param name="fileExtension">Расширение файла</param>
        /// <returns>Новый файл</returns>
        private ObjectFile CreateFile(BaseObject fileObject, string fileExtension)
        {
            if (fileObject == null) throw new ArgumentNullException("fileObject не может быть null");
            return new ObjectFile
            {
                ID = Guid.NewGuid(),
                Base_ID = fileObject.ID,
                Base = fileObject,
                FileExtension = fileExtension
            };
        }

        /// <summary>
        /// Создает новый атрибут системы
        /// </summary>
        /// <param name="attributeName">Имя атрибута</param>
        /// <param name="attributeObject">Объект атрибута</param>
        /// <param name="isReference">Ссылочный ли атрибут</param>
        /// <param name="referenceObjectType">Тип объекта, на который может ссылаться ссылочный атрибут</param>
        /// <param name="type">Тип данных атрибута</param>
        /// <returns>Новый атрибут</returns>
        private ObjectAttribute CreateAttribute(string attributeName, BaseObject attributeObject, bool isReference, ObjectType referenceObjectType, Type type)
        {
            if (attributeObject == null) throw new ArgumentNullException("attributeObject не может быть null");
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

        /// <summary>
        /// Создает новое действие в системе
        /// </summary>
        /// <param name="baseObject">Объект действия</param>
        /// <param name="commandName">Название команды действия</param>
        /// <param name="displayName">Отображаемое имя действия</param>
        /// <param name="types">Допустимые типы для действия</param>
        /// <returns>Новое действие</returns>
        private ObjectAction CreateAction(BaseObject objectOfAction, string commandName, string displayName, params ObjectType[] types)
        {
            if (objectOfAction == null) throw new ArgumentNullException("objectOfAction не может быть null");
            return new ObjectAction
            {
                ID = Guid.NewGuid(),
                Base_ID = objectOfAction.ID,
                Base = objectOfAction,
                Name = commandName,
                DisplayName = displayName,
                ObjectTypes = types
            };
        }

        /// <summary>
        /// Создает связь типа с атрибутом
        /// </summary>
        /// <param name="objectType">Тип объектов, к которым применим атрибут</param>
        /// <param name="attribute">Атрибут</param>
        /// <param name="isRequired">Обязателено ли наличие значения атрибута у объекта</param>
        /// <param name="isVisible">Отображается ли атрибут</param>
        /// <param name="isTitle">Входит ли атрибут в Title объекта</param>
        /// <param name="index">Порядок отображения атрибута у объекта</param>
        /// <returns>Связь типа с атрибутом</returns>
        private ObjectTypeAttribute CreateObjectTypeAttribute(ObjectType objectType, ObjectAttribute attribute, bool isRequired, bool isVisible, bool isTitle, int index)
        {
            if (objectType == null) throw new ArgumentNullException("objectType не может быть null"); 
            if (attribute == null) throw new ArgumentNullException("attribute не может быть null");
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

        /// <summary>
        /// Создает значение атрибута у объекта
        /// </summary>
        /// <param name="baseObject">Объект системы</param>
        /// <param name="attribute">Атрибут системы</param>
        /// <param name="value">Значение атрибута</param>
        /// <returns>Значение атрибута у объекта</returns>
        private ObjectAttributeValue CreateAttributeValue(BaseObject baseObject, ObjectAttribute attribute, string value)
        {
            if (baseObject == null) throw new ArgumentNullException("baseObject не может быть null");
            if (attribute == null) throw new ArgumentNullException("attribute не может быть null");
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