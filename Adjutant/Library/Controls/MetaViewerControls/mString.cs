﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Adjutant.Library.Cache;

namespace Adjutant.Library.Controls.MetaViewerControls
{
    internal partial class mString : MetaViewerControl
    {
        public int stringID;
        public string str;

        public mString(iValue Value, CacheFile Cache)
        {
            InitializeComponent();
            value = Value;
            cache = Cache;

            lblName.Text = value.Node.Attributes["name"].Value;
        }

        public override void Reload(int ParentAddress)
        {
            var reader = cache.Reader;

            int offset;

            try { offset = int.Parse(value.Node.Attributes["offset"].Value); }
            catch { offset = Convert.ToInt32(value.Node.Attributes["offset"].Value, 16); }

            reader.BaseStream.Position = ParentAddress + offset;

            switch (value.Type)
            {
                case iValue.ValueType.StringID:
                    stringID = reader.ReadInt32();
                    str = cache.Strings.GetItemByID(stringID);
                    txtValue.Text = str;
                    break;

                case iValue.ValueType.String:
                    int length = int.Parse(value.Node.Attributes["length"].Value);
                    txtValue.Text = reader.ReadNullTerminatedString(length);
                    break;

                default:
                    throw new InvalidOperationException("Cannot load " + value.Type.ToString() + " values using mString.");
            }
        }

        private void mString_DoubleClick(object sender, EventArgs e)
        {
            if(value.Type == iValue.ValueType.StringID)
                txtValue.Text = (txtValue.Text == str) ? stringID.ToString() : str;
        }
    }
}
