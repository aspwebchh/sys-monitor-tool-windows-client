﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace sys_monitor_tool {
    public class DetailPageDataBuilder {
        private DataTable dataTable;

        public DetailPageDataBuilder() {
            dataTable = new DataTable();
            dataTable.Columns.Add( new DataColumn( "Name", typeof( string ) ) );
            dataTable.Columns.Add( new DataColumn( "Value", typeof( string ) ) );
            dataTable.Columns.Add( new DataColumn( "Color", typeof( string ) ) );
        }

        public void Build( string name, string value ) {
            var row = dataTable.NewRow();
            row[ "Name" ] = name;
            row[ "Value" ] = value;
            row[ "Color" ] = Common.DEFAULT_TEXT_COLOR;
            dataTable.Rows.Add( row );
        }

        public DataTable DataSource {
            get {
                return dataTable;
            }
        }
    }
}
