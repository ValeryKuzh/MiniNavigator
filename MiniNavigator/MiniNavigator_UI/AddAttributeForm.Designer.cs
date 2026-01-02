namespace MiniNavigator_UI
{
    partial class AddAttributeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IsRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.IsVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.IsreferenceCheckBox = new System.Windows.Forms.CheckBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.NameAttributeTextBox = new System.Windows.Forms.TextBox();
            this.NameAttributeInfoLbl = new System.Windows.Forms.Label();
            this.ReferenceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ReferenceTypeInfoLbl = new System.Windows.Forms.Label();
            this.DataTypeInfoLbl = new System.Windows.Forms.Label();
            this.IsTitleCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // IsRequiredCheckBox
            // 
            this.IsRequiredCheckBox.AutoSize = true;
            this.IsRequiredCheckBox.Location = new System.Drawing.Point(12, 75);
            this.IsRequiredCheckBox.Name = "IsRequiredCheckBox";
            this.IsRequiredCheckBox.Size = new System.Drawing.Size(192, 24);
            this.IsRequiredCheckBox.TabIndex = 0;
            this.IsRequiredCheckBox.Text = "Атрибут обязателен";
            this.IsRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // IsVisibleCheckBox
            // 
            this.IsVisibleCheckBox.AutoSize = true;
            this.IsVisibleCheckBox.Location = new System.Drawing.Point(210, 75);
            this.IsVisibleCheckBox.Name = "IsVisibleCheckBox";
            this.IsVisibleCheckBox.Size = new System.Drawing.Size(211, 24);
            this.IsVisibleCheckBox.TabIndex = 1;
            this.IsVisibleCheckBox.Text = "Атрибут отображается";
            this.IsVisibleCheckBox.UseVisualStyleBackColor = true;
            // 
            // IsreferenceCheckBox
            // 
            this.IsreferenceCheckBox.AutoSize = true;
            this.IsreferenceCheckBox.Location = new System.Drawing.Point(12, 107);
            this.IsreferenceCheckBox.Name = "IsreferenceCheckBox";
            this.IsreferenceCheckBox.Size = new System.Drawing.Size(186, 24);
            this.IsreferenceCheckBox.TabIndex = 2;
            this.IsreferenceCheckBox.Text = "Атрибут ссылочный";
            this.IsreferenceCheckBox.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(222, 260);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(121, 34);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Отмена";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(353, 260);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(121, 34);
            this.OkBtn.TabIndex = 4;
            this.OkBtn.Text = "ОК";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // NameAttributeTextBox
            // 
            this.NameAttributeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameAttributeTextBox.Location = new System.Drawing.Point(12, 32);
            this.NameAttributeTextBox.Name = "NameAttributeTextBox";
            this.NameAttributeTextBox.Size = new System.Drawing.Size(462, 26);
            this.NameAttributeTextBox.TabIndex = 5;
            // 
            // NameAttributeInfoLbl
            // 
            this.NameAttributeInfoLbl.AutoSize = true;
            this.NameAttributeInfoLbl.Location = new System.Drawing.Point(12, 9);
            this.NameAttributeInfoLbl.Name = "NameAttributeInfoLbl";
            this.NameAttributeInfoLbl.Size = new System.Drawing.Size(157, 20);
            this.NameAttributeInfoLbl.TabIndex = 6;
            this.NameAttributeInfoLbl.Text = "Название атрибута";
            // 
            // ReferenceTypeComboBox
            // 
            this.ReferenceTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceTypeComboBox.FormattingEnabled = true;
            this.ReferenceTypeComboBox.Location = new System.Drawing.Point(12, 161);
            this.ReferenceTypeComboBox.Name = "ReferenceTypeComboBox";
            this.ReferenceTypeComboBox.Size = new System.Drawing.Size(462, 28);
            this.ReferenceTypeComboBox.TabIndex = 7;
            // 
            // DataTypeComboBox
            // 
            this.DataTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataTypeComboBox.FormattingEnabled = true;
            this.DataTypeComboBox.Location = new System.Drawing.Point(12, 220);
            this.DataTypeComboBox.Name = "DataTypeComboBox";
            this.DataTypeComboBox.Size = new System.Drawing.Size(462, 28);
            this.DataTypeComboBox.TabIndex = 8;
            // 
            // ReferenceTypeInfoLbl
            // 
            this.ReferenceTypeInfoLbl.AutoSize = true;
            this.ReferenceTypeInfoLbl.Location = new System.Drawing.Point(12, 138);
            this.ReferenceTypeInfoLbl.Name = "ReferenceTypeInfoLbl";
            this.ReferenceTypeInfoLbl.Size = new System.Drawing.Size(113, 20);
            this.ReferenceTypeInfoLbl.TabIndex = 9;
            this.ReferenceTypeInfoLbl.Text = "Тип объектов";
            // 
            // DataTypeInfoLbl
            // 
            this.DataTypeInfoLbl.AutoSize = true;
            this.DataTypeInfoLbl.Location = new System.Drawing.Point(12, 197);
            this.DataTypeInfoLbl.Name = "DataTypeInfoLbl";
            this.DataTypeInfoLbl.Size = new System.Drawing.Size(172, 20);
            this.DataTypeInfoLbl.TabIndex = 10;
            this.DataTypeInfoLbl.Text = "Тип хранимых данных";
            // 
            // IsTitleCheckBox
            // 
            this.IsTitleCheckBox.AutoSize = true;
            this.IsTitleCheckBox.Location = new System.Drawing.Point(210, 107);
            this.IsTitleCheckBox.Name = "IsTitleCheckBox";
            this.IsTitleCheckBox.Size = new System.Drawing.Size(252, 24);
            this.IsTitleCheckBox.TabIndex = 11;
            this.IsTitleCheckBox.Text = "Атрибут используется в Title";
            this.IsTitleCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddAttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 305);
            this.Controls.Add(this.IsTitleCheckBox);
            this.Controls.Add(this.DataTypeInfoLbl);
            this.Controls.Add(this.ReferenceTypeInfoLbl);
            this.Controls.Add(this.DataTypeComboBox);
            this.Controls.Add(this.ReferenceTypeComboBox);
            this.Controls.Add(this.NameAttributeInfoLbl);
            this.Controls.Add(this.NameAttributeTextBox);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.IsreferenceCheckBox);
            this.Controls.Add(this.IsVisibleCheckBox);
            this.Controls.Add(this.IsRequiredCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddAttributeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить атрибут";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox IsRequiredCheckBox;
        private System.Windows.Forms.CheckBox IsVisibleCheckBox;
        private System.Windows.Forms.CheckBox IsreferenceCheckBox;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.TextBox NameAttributeTextBox;
        private System.Windows.Forms.Label NameAttributeInfoLbl;
        private System.Windows.Forms.ComboBox ReferenceTypeComboBox;
        private System.Windows.Forms.ComboBox DataTypeComboBox;
        private System.Windows.Forms.Label ReferenceTypeInfoLbl;
        private System.Windows.Forms.Label DataTypeInfoLbl;
        private System.Windows.Forms.CheckBox IsTitleCheckBox;
    }
}