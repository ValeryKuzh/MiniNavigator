using Infralution.Controls.VirtualTree;

namespace MiniNavigator_UI
{
    partial class NavigatorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infralution.Controls.VirtualTree.ObjectCellBinding objectCellBinding = new Infralution.Controls.VirtualTree.ObjectCellBinding();
            this.colMain = new Infralution.Controls.VirtualTree.Column();
            this.NavigatorSplitContainer = new System.Windows.Forms.SplitContainer();
            this.NavigatorVirtualTree = new Infralution.Controls.VirtualTree.VirtualTree();
            this.rowBindingNavObjectViewModel = new Infralution.Controls.VirtualTree.ObjectRowBinding();
            this.NavigatorDataGridView = new System.Windows.Forms.DataGridView();
            this.NavObjectViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).BeginInit();
            this.NavigatorSplitContainer.Panel1.SuspendLayout();
            this.NavigatorSplitContainer.Panel2.SuspendLayout();
            this.NavigatorSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavObjectViewModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // colMain
            // 
            this.colMain.AutoSizePolicy = Infralution.Controls.VirtualTree.ColumnAutoSizePolicy.AutoIncrease;
            this.colMain.Caption = "Объекты";
            this.colMain.DataField = "__MAIN__";
            this.colMain.Hidable = false;
            this.colMain.MinWidth = 45;
            this.colMain.Name = "colMain";
            this.colMain.Width = 862;
            // 
            // NavigatorSplitContainer
            // 
            this.NavigatorSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.NavigatorSplitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorSplitContainer.Name = "NavigatorSplitContainer";
            this.NavigatorSplitContainer.SplitterMoved += NavigatorSplitContainer_SplitterMoved;
            // 
            // NavigatorSplitContainer.Panel1
            // 
            this.NavigatorSplitContainer.Panel1.Controls.Add(this.NavigatorVirtualTree);
            // 
            // NavigatorSplitContainer.Panel2
            // 
            this.NavigatorSplitContainer.Panel2.Controls.Add(this.NavigatorDataGridView);
            this.NavigatorSplitContainer.Size = new System.Drawing.Size(1178, 705);
            this.NavigatorSplitContainer.SplitterDistance = 382;
            this.NavigatorSplitContainer.SplitterWidth = 6;
            this.NavigatorSplitContainer.TabIndex = 1;
            // 
            // NavigatorVirtualTree
            // 
            this.NavigatorVirtualTree.Columns.Add(this.colMain);
            this.NavigatorVirtualTree.ConnectionOffset = 12;
            this.NavigatorVirtualTree.DataSource = this.NavObjectViewModelBindingSource;
            this.NavigatorVirtualTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorVirtualTree.HeaderHeight = 27;
            this.NavigatorVirtualTree.IndentWidth = 30;
            this.NavigatorVirtualTree.Location = new System.Drawing.Point(0, 0);
            this.NavigatorVirtualTree.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorVirtualTree.MaxRowHeight = 184;
            this.NavigatorVirtualTree.MinRowHeight = 21;
            this.NavigatorVirtualTree.Name = "NavigatorVirtualTree";
            this.NavigatorVirtualTree.RowBindings.Add(this.rowBindingNavObjectViewModel);
            this.NavigatorVirtualTree.RowHeaderWidth = 45;
            this.NavigatorVirtualTree.RowHeight = 27;
            this.NavigatorVirtualTree.ScalingFactor = 1.5F;
            this.NavigatorVirtualTree.Size = new System.Drawing.Size(382, 705);
            this.NavigatorVirtualTree.TabIndex = 0;
            // 
            // rowBindingNavObjectViewModel
            // 
            objectCellBinding.Column = this.colMain;
            this.rowBindingNavObjectViewModel.CellBindings.Add(objectCellBinding);
            this.rowBindingNavObjectViewModel.Name = "rowBindingNavObjectViewModel";
            this.rowBindingNavObjectViewModel.TypedListName = "NavObjectViewModel";
            // 
            // NavigatorDataGridView
            // 
            this.NavigatorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NavigatorDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigatorDataGridView.Location = new System.Drawing.Point(0, 0);
            this.NavigatorDataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NavigatorDataGridView.Name = "NavigatorDataGridView";
            this.NavigatorDataGridView.RowHeadersWidth = 62;
            this.NavigatorDataGridView.Size = new System.Drawing.Size(790, 705);
            this.NavigatorDataGridView.TabIndex = 0;
            // 
            // NavObjectViewModelBindingSource
            // 
            this.NavObjectViewModelBindingSource.DataSource = typeof(MiniNavigator_UI.ViewModel.NavObjectViewModel);
            // 
            // NavigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 705);
            this.Controls.Add(this.NavigatorSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NavigatorForm";
            this.Text = "MiniNavigator";
            this.NavigatorSplitContainer.Panel1.ResumeLayout(false);
            this.NavigatorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorSplitContainer)).EndInit();
            this.NavigatorSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorVirtualTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavigatorDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavObjectViewModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        private void NavigatorSplitContainer_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            this.colMain.Width = this.NavigatorSplitContainer.Panel1.Width;
        }

        #endregion

        private Infralution.Controls.VirtualTree.VirtualTree NavigatorVirtualTree;
        private System.Windows.Forms.BindingSource NavObjectViewModelBindingSource;
        private Infralution.Controls.VirtualTree.Column colMain;
        private Infralution.Controls.VirtualTree.ObjectRowBinding rowBindingNavObjectViewModel;
        private System.Windows.Forms.SplitContainer NavigatorSplitContainer;
        private System.Windows.Forms.DataGridView NavigatorDataGridView;
    }

}

