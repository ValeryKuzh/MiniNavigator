using Infralution.Controls.VirtualTree;
using MiniNavigator_Services.DTO;
using MiniNavigator_Services.Service;
using MiniNavigator_Services.Service.Interface;
using MiniNavigator_UI.Mapper.Interface;
using MiniNavigator_UI.Service;
using MiniNavigator_UI.Service.ActionHandler;
using MiniNavigator_UI.Service.ActionHandler.Handler;
using MiniNavigator_UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MiniNavigator_UI
{
    /// <summary>
    /// Основная форма приложения
    /// </summary>
    public partial class NavigatorForm : Form
    {
        private readonly IObjectService _objectService;
        private readonly IObjectTypeService _objectTypeService;
        private readonly IFileService _fileService;
        private readonly IAttributeService _attributeService;
        private readonly IValidationService _validationService;

        private readonly ActionHandlerRegistry _actionRegistry;
        private readonly IServiceProvider _serviceProvider;

        private readonly IMapper<NavObjectViewModel, NavObjectDTO> _objectMapper;

        private NavObjectViewModel _treeOfObjects = new NavObjectViewModel();

        private BindingList<DynamicObjectRow> _rows;
        private List<ObjectAttributeViewModel> _attributes;
        private Guid _currentTypeId;


        public NavigatorForm
            (
            IObjectService objectService,
            IObjectTypeService objectTypeService,
            IFileService fileService,
            IAttributeService attributeService,
            IValidationService validationService,

            ActionHandlerRegistry actionRegistry,
            IServiceProvider serviceProvider,

            IMapper<NavObjectViewModel, NavObjectDTO> objectMapper
            )
        {
            _objectService = objectService;
            _objectTypeService = objectTypeService;
            _fileService = fileService;
            _attributeService = attributeService;
            _validationService = validationService;

            _actionRegistry = actionRegistry;
            _serviceProvider = serviceProvider;
            
            _objectMapper = objectMapper;

            InitializeComponent();

            NavigatorDataGridView.RowHeaderMouseClick += NavigatorDataGridView_RowHeaderMouseClick;
            NavigatorDataGridView.CellClick += NavigatorDataGridView_CellClick;
            NavigatorDataGridView.CellValidating += NavigatorDataGridView_CellValidating;
            NavigatorDataGridView.CellPainting += NavigatorDataGridView_CellPainting;
            NavigatorDataGridView.CellBeginEdit += NavigatorDataGridView_CellBeginEdit;


            this.Load += NavigatorForm_Load;
        }

        /// <summary>
        /// Отменять редактирование, если не в режиме редактирования
        /// </summary>
        private void NavigatorDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!IsEditing)
                e.Cancel = true;
        }

        /// <summary>
        /// Отрисовка кнопки поверх ячейки
        /// </summary>
        private void NavigatorDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null || (!attr.IsReference && attr.ValueType != typeof(DateTime)))
                return;

            var row = _rows[e.RowIndex];

            if (row != _editingRow || !IsEditing)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);
                return;
            }

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            var text = row.Attributes[attr.ID].Value;
            if (string.IsNullOrWhiteSpace(text))
                text = "Выбрать...";

            var rect = new Rectangle(
                e.CellBounds.X + 2,
                e.CellBounds.Y + 2,
                e.CellBounds.Width - 4,
                e.CellBounds.Height - 4
            );

            ButtonRenderer.DrawButton(
                e.Graphics,
                rect,
                text,
                NavigatorDataGridView.Font,
                false,
                PushButtonState.Normal
            );
        }

        /// <summary>
        /// Валидация значений в ячейках
        /// </summary>
        private void NavigatorDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!IsEditing || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];

            var attr = column.Tag as ObjectAttributeViewModel;
            if (attr == null)
                return;

            if (attr.IsReference)
                return;

            var value = e.FormattedValue?.ToString();
            if (string.IsNullOrWhiteSpace(value))
                return;

            string error;
            bool valid = _validationService.ValidateSingleValue(
                value,
                attr.ValueType,
                out error
            );

            if (!valid)
            {
                e.Cancel = true;
                MessageBox.Show(
                    error,
                    "Ошибка ввода",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик загрузки формы
        /// </summary>
        private async void NavigatorForm_Load(object sender, EventArgs e)
        {
            BindTree();

            await InitRootAsync();

            NavigatorVirtualTree.DataSource = _treeOfObjects;
            NavigatorVirtualTree.Refresh();
        }

        /// <summary>
        /// Биндинг на дерево
        /// </summary>
        private void BindTree()
        {
            var binding = new ObjectRowBinding(typeof(NavObjectViewModel));

            binding.ChildProperty = nameof(NavObjectViewModel.Children);

            var nameBinding = new ObjectCellBinding
            {
                Column = NavigatorVirtualTree.Columns[0],
                Field = nameof(NavObjectViewModel.ObjectTitle)
            };

            binding.CellBindings.Add(nameBinding);

            NavigatorVirtualTree.RowBindings.Add(binding);
        }

        /// <summary>
        /// Инициализация дерева
        /// </summary>
        private async Task InitRootAsync()
        {
            var rootDto = await _objectService.GetTreeOfObjectsAsync();
            _treeOfObjects = ConvertTreeToViewModels(rootDto);
        }

        /// <summary>
        /// Конвертация элементов дерева из DTO во ViewModel
        /// </summary>
        /// <param name="root">Корень дерева DTO</param>
        /// <returns>Корень дерева ViewModel</returns>
        public NavObjectViewModel ConvertTreeToViewModels(NavObjectDTO root)
        {
            var result = _objectMapper.ToViewModel(root);

            if (root.Children != null && root.Children.Any())
            {
                foreach (var child in root.Children)
                {
                    result.Children.Add(ConvertTreeToViewModels(child));
                }
            }

            return result;
        }

        /// <summary>
        /// Обработчик нажатия мыши по дереву
        /// </summary>
        private async void NavigatorVirtualTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsEditing)
                return;
            if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectViewModel navObjectViewModel)
            {

                if (navObjectViewModel.Children.Count == 0) {
                    if (e.Button == MouseButtons.Right)
                    {
                        await ShowContextMenuAsync(e.Location, navObjectViewModel);
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        await BindTableAsync(navObjectViewModel.ID);
                    }
                }
                else
                {
                    NavigatorDataGridView.Columns.Clear();
                }
            }
        }

        /// <summary>
        /// Отрисовка контекстного меню для типов 
        /// </summary>
        /// <param name="location">Точка нажатия на форму</param>
        /// <param name="vm">Объект, по которому кликнули</param>
        private async Task ShowContextMenuAsync(Point location, NavObjectViewModel vm)
        {
            if (vm == null) throw new ArgumentNullException(nameof(vm)); 
            if (location == null) throw new ArgumentNullException(nameof(location));

            var type = await _objectTypeService
                .GetTypeByTypeObjectIDAsync(vm.ID);

            ContextMenuStrip menu = new ContextMenuStrip();

            if (await _objectTypeService.IsFileTypeAsync(type.ID))
            {
                var addItem = new ToolStripMenuItem("Добавить файл");
                addItem.Click += async (s, e) =>
                {
                    AddItem_Click(s, e);
                    await FileAdd_Click(type);
                };
                menu.Items.Add(addItem);
            }
            else
            {
                var addItem = new ToolStripMenuItem("Добавить");
                addItem.Click += AddItem_Click;
                menu.Items.Add(addItem);
            }

            var addAttributeItem = new ToolStripMenuItem("Добавить атрибут");
            addAttributeItem.Click += async (s, e) =>
            { 
                await AddAttributeItem_Click(vm.ID); 
            };
            menu.Items.Add(addAttributeItem);

            menu.Show(NavigatorVirtualTree, location);
        }

        /// <summary>
        /// Обработчик добавления атрибута
        /// </summary>
        /// <param name="typeID">ID обхекта типа</param>
        private async Task AddAttributeItem_Click(Guid typeID)
        {
            if (typeID == null) throw new ArgumentNullException(nameof(typeID));

            var allObjectTypes = await _objectTypeService.GetAllTypesAsync();
            
            var list = new List<NavObjectViewModel>();
            
            GetAllObjectsTypesFromTree(_treeOfObjects, list);
            var objectTypesIDs = list
                                .Where(n => n.ID != typeID)
                                .Select(n => n.ID)
                                .Distinct()
                                .ToList();

            var objectTypes = allObjectTypes.Where(t => objectTypesIDs.Contains(t.ObjectID)).ToList();

            using (var form = new AddAttributeForm(objectTypes))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var attributeDto = new ObjectAttributeDTO
                    {
                        ID = Guid.NewGuid(),
                        Name = form.AttributeName,
                        IsReference = form.IsReference,
                        ValueType = form.SelectedValueType ?? null,
                        IsRequired = form.IsRequired,
                        IsVisible = form.IsVisible,
                        IsTitle = form.IsTitle,
                        ReferenceObjectTypeID = form.SelectedReferenceTypeId ?? null,
                        Index = (await GetAttributesForType(typeID)).Count + 1
                    };
                    var type = allObjectTypes.Where(ot => ot.ObjectID == typeID).First();

                    await _attributeService.CreateAttributeAsync(attributeDto, type.ID);

                    await BindTableAsync(typeID);
                }
            }
        }

        /// <summary>
        /// Получение типов из дерева в виде списка
        /// </summary>
        /// <param name="root">Корень дерева</param>
        /// <param name="list">Список для добавления типов</param>
        public void GetAllObjectsTypesFromTree(NavObjectViewModel root, List<NavObjectViewModel> list)
        {
            if (root.Children != null && root.Children.Count != 0)
            {
                foreach (var node in root.Children)
                {
                    GetAllObjectsTypesFromTree(node, list);
                }
            }
            else
            {
                list.Add(root);
            }
        }

        /// <summary>
        /// Обработчик добавления файла
        /// </summary>
        /// <param name="type">Обьект файла</param>
        private async Task FileAdd_Click(ObjectTypeDTO type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var filter = await _fileService.GetOpenFileDialogFilterAsync(type.ID);

            using (var openFileDialog = new OpenFileDialog
            {
                Title = $"Выберите {type.Title} файл",
                Filter = filter,
                Multiselect = false
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;

                    var fileAttr = _editingRow.Attributes
                        .Values
                        .FirstOrDefault(a => a.Name.ToLower().Contains("File"));

                    if (fileAttr != null)
                    {
                        fileAttr.Value = filePath;
                    }
                    else
                    {
                        _editingRow.Attributes[type.ID] = new ObjectAttributeViewModel
                        {
                            ID = type.ID,
                            Name = "File",
                            ValueType = typeof(string),
                            Value = filePath
                        };
                    }

                    NavigatorDataGridView.Invalidate();
                }
            }
        }

        /// <summary>
        /// Обработчик добавления нового объекта
        /// </summary>
        private void AddItem_Click(object sender, EventArgs e)
        {
            if (NavigatorVirtualTree.SelectedRow?.Item is NavObjectViewModel && _rows != null)
            {
                ResetSorting();
                SetTableReadonlyProperty();

                AddNewRow();

                IsEditing = true;
                InitializeCreateNewObject();
                ShowControls();
            }
        }

        /// <summary>
        /// Добавление новой строки в таблице
        /// </summary>
        private void AddNewRow()
        {
            var row = new DynamicObjectRow();

            foreach (var attr in _attributes)
            {
                row.Attributes[attr.ID] = new ObjectAttributeViewModel
                {
                    ID = attr.ID,
                    Name = attr.Name,
                    ValueType = attr.ValueType,
                    IsReference = attr.IsReference,
                    IsRequired = attr.IsRequired,
                    Value = null
                };
            }

            if (NavigatorDataGridView.ColumnCount != 0)
            {
                _rows.Add(row);
                _editingRow = row;
                _isNewRow = true;


                int lastRowIndex = NavigatorDataGridView.Rows.Count - 1;
                if (lastRowIndex >= 0)
                {
                    NavigatorDataGridView.CurrentCell =
                        NavigatorDataGridView.Rows[lastRowIndex].Cells[0];
                }

                NavigatorDataGridView.BeginEdit(true);
            }
        }

        /// <summary>
        /// Удаление строки из таблицы
        /// </summary>
        /// <param name="row">Удаляемая строка</param>
        public void RemoveRow(DynamicObjectRow row)
        {
            if (row == null || _rows == null)
                return;

            _rows.Remove(row);
            NavigatorDataGridView.Invalidate();
        }

        /// <summary>
        /// Сброс сортировки
        /// </summary>
        private void ResetSorting()
        {
            foreach (DataGridViewColumn col in NavigatorDataGridView.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// Биндинг объектов системы на DataGridView
        /// </summary>
        /// <param name="typeId">Тип объекта</param>
        private async Task BindTableAsync(Guid typeId)
        {
            if(typeId == Guid.Empty) throw new ArgumentNullException(nameof(typeId));

            _currentTypeId = typeId;

            _attributes = await GetAttributesForType(typeId);

            ConfigureGrid();
            CreateColumns(_attributes);

            var tableData = await _objectService.GetTableData(typeId);

            _rows = await CreateRowsAsync(tableData);

            BindGrid(_rows);

            IsEditing = false;
        }

        /// <summary>
        /// Получение атрибутов для типа
        /// </summary>
        /// <param name="typeID">Тип объекта</param>
        /// <returns>Список атрибутов</returns>
        public async Task<List<ObjectAttributeViewModel>> GetAttributesForType(Guid typeID)
        {
            if (typeID == Guid.Empty) throw new ArgumentNullException(nameof(typeID));

            var attributes = await _objectTypeService.GetAttributesForTypeAsync(typeID);

            return attributes
                .Where(a => a.IsVisible)
                .OrderBy(a => a.Index)
                .Select(a => new ObjectAttributeViewModel
                {
                    ID = a.ID,
                    Name = a.Name,
                    ValueType = a.ValueType,
                    IsReference = a.IsReference,
                    IsRequired = a.IsRequired
                })
                .ToList();
        }

        /// <summary>
        /// Конфигурация DataGridView
        /// </summary>
        private void ConfigureGrid()
        {
            NavigatorDataGridView.AutoGenerateColumns = false;
            NavigatorDataGridView.Columns.Clear();
            NavigatorDataGridView.AllowUserToAddRows = false;
            NavigatorDataGridView.ReadOnly = false;

            NavigatorDataGridView.VirtualMode = true;
            NavigatorDataGridView.CellValueNeeded += NavigatorDataGridView_CellValueNeeded;
            NavigatorDataGridView.CellValuePushed += NavigatorDataGridView_CellValuePushed;
        }


        /// <summary>
        /// Создание колонок в таблице
        /// </summary>
        /// <param name="attributes">ViewModel объекты атрибутов</param>
        private void CreateColumns(List<ObjectAttributeViewModel> attributes)
        {
            if(attributes == null) throw new ArgumentNullException(nameof(attributes));

            foreach (var attr in attributes)
            {
                var column = CreateColumn(attr);
                NavigatorDataGridView.Columns.Add(column);
            }
        }

        /// <summary>
        /// Создание одной колонки 
        /// </summary>
        /// <param name="attr">ViewModel объект атрибута</param>
        /// <returns>Колонка DataGridView</returns>
        private DataGridViewColumn CreateColumn(ObjectAttributeViewModel attr)
        {
            if(attr == null) throw new ArgumentNullException(nameof(attr));

            DataGridViewColumn column;

            if (attr.ValueType == typeof(bool))
            {
                column = new DataGridViewCheckBoxColumn();
            }
            else
            {
                column = new DataGridViewTextBoxColumn();
            }

            column.Name = attr.Name;
            column.HeaderText = attr.Name;
            column.Tag = attr;
            column.ReadOnly = false;

            return column;
        }

        /// <summary>
        /// Создание строк
        /// </summary>
        /// <param name="tableData">Данные об объектах из сервиса</param>
        /// <returns>Структура для отображения на DataGridView</returns>
        private async Task<BindingList<DynamicObjectRow>> CreateRowsAsync(List<Dictionary<Guid, ObjectAttributeDTO>> tableData)
        {
            if(tableData == null) throw new ArgumentNullException(nameof(tableData));
            
            var rows = new BindingList<DynamicObjectRow>();

            foreach (var rowDict in tableData)
            {
                var row = await CreateRowAsync(rowDict);
                rows.Add(row);
            }

            return rows;
        }

        /// <summary>
        /// Создание строки
        /// </summary>
        /// <param name="rowDict">Данные о строке</param>
        /// <returns>Строка для DataGridView</returns>
        private async Task<DynamicObjectRow> CreateRowAsync(Dictionary<Guid, ObjectAttributeDTO> rowDict)
        {
            if (rowDict == null) throw new ArgumentNullException(nameof(rowDict));

            var row = new DynamicObjectRow();

            var idAttr = rowDict.Values.FirstOrDefault(a => a.Name == "ID");
            if (idAttr != null && Guid.TryParse(idAttr.Value, out var objectId))
            {
                row.ObjectId = objectId;
            }

            foreach (var attr in rowDict.Values)
            {
                if (attr == null || !attr.IsVisible)
                    continue;

                string displayValue = null;
                Guid? referenceId = null;

                if (attr.IsReference && !string.IsNullOrWhiteSpace(attr.Value))
                {
                    referenceId = Guid.Parse(attr.Value);

                    var obj = await _objectService.GetObjectInfoByIdAsync(referenceId.Value);
                    displayValue = obj?.Title;
                }
                else
                {
                    displayValue = attr.Value;
                }

                row.Attributes[attr.ID] = new ObjectAttributeViewModel
                {
                    ID = attr.ID,
                    Name = attr.Name,
                    ValueType = attr.ValueType,
                    IsReference = attr.IsReference,
                    IsRequired = attr.IsRequired,

                    Value = displayValue,
                    ReferenceID = referenceId
                };
            }

            return row;
        }

        /// <summary>
        /// Обработчик для отображения значения в ячейке
        /// </summary>
        private void NavigatorDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _rows.Count)
                return;

            var row = _rows[e.RowIndex];
            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;
            if (attr == null)
                return;

            if (row.Attributes.TryGetValue(attr.ID, out var avm))
                e.Value = avm.Value;
        }

        /// <summary>
        /// Обработчик для получения значения из ячейки
        /// </summary>
        private void NavigatorDataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (!IsEditing)
                return;

            var row = _rows[e.RowIndex];

            if (row != _editingRow)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null)
                return;

            if (attr.IsReference)
                return;

            if (row.Attributes.TryGetValue(attr.ID, out var vm))
                vm.Value = e.Value?.ToString();
        }

        /// <summary>
        /// Биндинг DataGridView 
        /// </summary>
        /// <param name="rows">Струтура для биндинга</param>
        private void BindGrid(BindingList<DynamicObjectRow> rows)
        {
            NavigatorDataGridView.DataSource = rows;
        }

        /// <summary>
        /// Обработчик нажатия мышью по заголовку строки
        /// </summary>
        private async void NavigatorDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            NavigatorDataGridView.ClearSelection();
            NavigatorDataGridView.Rows[e.RowIndex].Selected = true;

            if (e.Button != MouseButtons.Right)
                return;

            var row = _rows[e.RowIndex];
            if (row == null)
                return;

            // Показываем контекстное меню
            await ShowContextMenuForRowAsync(row, NavigatorDataGridView.PointToClient(Cursor.Position));
        }

        /// <summary>
        /// Отображение контекстного меню для объекта
        /// </summary>
        /// <param name="row">Выбранная строка</param>
        /// <param name="location">Точка по которой кликнули</param>
        private async Task ShowContextMenuForRowAsync(DynamicObjectRow row, Point location)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var objectId = row.ObjectId;
            var actions = await _objectService.GetActionsForTypeObject(objectId);

            if (actions == null || actions.Count == 0)
                return;

            ContextMenuStrip menu = new ContextMenuStrip();

            foreach (var action in actions)
            {
                var item = new ToolStripMenuItem(action.DisplayName) { Tag = action };
                menu.Items.Add(item);
                item.Click += (s, e) => ExecuteAction(row, action);
            }

            menu.Show(NavigatorDataGridView, location);
        }

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="row">Выбранная строка</param>
        /// <param name="action">DTO действия</param>
        private async void ExecuteAction(DynamicObjectRow row, ObjectActionDTO action)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            var handler = _actionRegistry.Resolve(action.CommandName);

            var context = new ActionContext
            {
                CommandName = action.CommandName,
                Row = row,
                ObjectTypeId = _currentTypeId,
                Services = _serviceProvider
            };

            if (handler.CanExecute(context))
                await handler.ExecuteAsync(context);
        }


        /// <summary>
        /// Обработчик для выделения отдельной ячейки
        /// </summary>
        private void NavigatorDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                NavigatorDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
                NavigatorDataGridView.ClearSelection();
                NavigatorDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }

            if (!IsEditing || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var row = _rows[e.RowIndex];
            if (row != _editingRow)
                return;

            var column = NavigatorDataGridView.Columns[e.ColumnIndex];
            var attr = column.Tag as ObjectAttributeViewModel;

            if (attr == null || (!attr.IsReference && attr.ValueType != typeof(DateTime)))
                return;

            if (row == _editingRow && IsEditing)
            {
                if (attr.IsReference)
                {
                    using (var form = new ReferenceSelectForm(attr.ID, _objectService))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            var vm = row.Attributes[attr.ID];

                            vm.Value = form.SelectedTitle;
                            vm.ReferenceID = form.SelectedID;

                            NavigatorDataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                        }
                    }
                }
                else if(attr.ValueType == typeof(DateTime))
                {
                    if (!DateTime.TryParse(attr.Value, out DateTime value))
                    {
                        value = DateTime.Now;
                    }
                    using (var form = new DateTimeEditForm(value))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            row.Attributes[attr.ID].Value = form.SelectedDate.ToString("yyyy-MM-dd");
                            NavigatorDataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Начало редатирования строки
        /// </summary>
        /// <param name="row">Редактируемая строка</param>
        public void BeginEditRow(DynamicObjectRow row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            _editingRow = row;
            IsEditing = true;
            _isNewRow = false;

            ResetSorting();
            SetTableReadonlyProperty();
            InitializeCreateNewObject();
            ShowControls();

            int rowIndex = _rows.IndexOf(row);
            if (rowIndex >= 0)
            {
                NavigatorDataGridView.CurrentCell = NavigatorDataGridView.Rows[rowIndex].Cells[0];
                NavigatorDataGridView.BeginEdit(true);
            }
        }
    }
}