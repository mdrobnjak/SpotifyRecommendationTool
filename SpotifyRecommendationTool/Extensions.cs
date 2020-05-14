using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyRecommendationTool
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a dynamic value dictionary from a DataGridViewRow. DataGridViewColumns must have Tag set to desired value type.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> ToDict(this DataGridViewRow row)
        {
            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();

            foreach (DataGridViewCell cell in row.Cells)
            {
                var columnType = (Type)cell.DataGridView.Columns[cell.ColumnIndex].Tag;

                if (cell.Value == null)
                {
                    cell.Value = columnType.Name == "String" ? "" : Activator.CreateInstance(columnType);
                }

                dynamic value = Convert.ChangeType(cell.Value, columnType);

                d.Add(row.DataGridView.Columns[cell.ColumnIndex].Name, value);
            }

            return d;
        }

        /// <summary>
        /// Returns unique and non-empty formatted cell values.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static List<string> GetUniqueValues(this DataGridViewColumn col)
        {
            return col.DataGridView.Rows
                                     .OfType<DataGridViewRow>()
                                     .Select(r => r.Cells[col.Name].FormattedValue.ToString())
                                     .Where(s => !string.IsNullOrWhiteSpace(s))
                                     .Distinct()
                                     .ToList();
        }
    }
}
