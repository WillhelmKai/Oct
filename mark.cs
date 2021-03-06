﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Evaluate
{
    public partial class mark : Form
    {
        public string name;
        public string course;
        public string taskname;
        public int count=0;
        public int sumcount;
        public bool flag=true;
        public mark()
        {
            InitializeComponent();
        }
        public DataGridViewRow dgvr;
        public mark(DataGridViewRow myDGVR)
        {
            InitializeComponent();
            dgvr = myDGVR;
            name= dgvr.Cells[2].Value.ToString();
            taskname = dgvr.Cells[1].Value.ToString();
            course = dgvr.Cells[0].Value.ToString();
            
            // hello
        }

        public XmlAttribute CreateAttribute(XmlNode node, string attributeName, string value)
        {
            try
            {
                XmlDocument doc = node.OwnerDocument;
                XmlAttribute attr = null;
                attr = doc.CreateAttribute(attributeName);
                attr.Value = value;
                node.Attributes.SetNamedItem(attr);
                return attr;
            }
            catch (Exception err)
            {
                string desc = err.Message;
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Evaluate f1 = new Evaluate();
            f1.Show();
            this.Hide();
        }

        private void mark_Load(object sender, EventArgs e)
        {
            string xmlpath = @"rubric.xml";
            XElement root = XElement.Load(xmlpath);
            IEnumerable<XElement> address =
               from el in root.Elements("courses")
               where (string)el.Attribute("name") == name
               select el;
            var query = from n in address.Elements()
                        select new
                        {
                            Item = n.Attribute("Item").Value,
                            A = n.Attribute("A").Value,
                            B = n.Attribute("B").Value,
                            C = n.Attribute("C").Value,
                            D = n.Attribute("D").Value,
                            F = n.Attribute("F").Value
                        };
            dataGridView1.DataSource = query.ToList();


           string strPath = @"student.xml";
            XElement student = XElement.Load(strPath);
            IEnumerable<XElement> stu = from st in student.Elements("student")
                                        where (string)st.Attribute("course") == course
                                        select st;
            var query1 = from n in stu.Elements()
                         select new
                         {
                             Studentname = n.Attribute("id").Value
                         };
            dataGridView2.DataSource = query1.ToList();
            
            //extract information in rubric
            XmlDocument doc = new XmlDocument();
            doc.Load("rubric.xml");
            string[] array = new string[100];
            string stupath = "//courses[@name='" + name + "']//task";
            XmlNodeList nodeList = doc.SelectNodes(stupath);
            foreach (XmlNode item in nodeList)
            {
                array[count] = item.Attributes[1].Value;
                count++;
            }
            sumcount = count+1;
            for (;count>0;count--)
            {
                DataGridViewComboBoxColumn dgcbc = new DataGridViewComboBoxColumn();
                DataGridViewComboBoxCell it = new DataGridViewComboBoxCell();
                it.Items.Add("A");
                it.Items.Add("B");
                it.Items.Add("C");
                it.Items.Add("D");
                it.Items.Add("F");
                dgcbc.CellTemplate = it;
                //设置列的属性
                dgcbc.DataPropertyName = array[count-1];
                dgcbc.Name = array[count-1];
                dgcbc.HeaderText = array[count-1];
                dataGridView2.Columns.Add(dgcbc);
            }
            DataGridViewTextBoxColumn sum = new DataGridViewTextBoxColumn();
            sum.Name = "sum";
            sum.DataPropertyName = "sum";
            sum.HeaderText = "sum";
            dataGridView2.Columns.Add(sum);
            for(int i=0;i<dataGridView2.RowCount;i++)
            {
                dataGridView2.Rows[i].Cells[dataGridView2.ColumnCount - 1].Value = 0;
            }


        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("你选定的项为:" + dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString());
            int column = dataGridView2.ColumnCount;
            double average = Convert.ToDouble(column-2);
            if(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Equals("A"))
            {
                dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value) + (100.0 / average);





                /*if((change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average))<20)
                {
                    dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = "F";
                }
               else if((change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average)) <40 && (change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average)) > 20)
                {
                    dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = "D";
                }
                else if((change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average)) < 60 && (change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average)) > 40)
                {
                    dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = "C";
                }
                else if((change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) < 80 && (change(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value.ToString()) + (100.0 / average)) > 60))
                {
                    dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = "B";
                }
                
                else
                {
                    dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = "A";
                }*/
            }
            else if(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Equals("B"))
            {
                dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value) + (80.0 / average);
            }
            else if(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Equals("C"))
            {
                dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value) + (60.0 / average);
            }
            else if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString().Equals("D"))
            {
                dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value) + (40.0 / average);
            }
            else
            {
                dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value = Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[column - 1].Value) + (20.0 / average);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //create the root element
            XmlElement root = doc.CreateElement("UIC");
            doc.AppendChild(root);
            //create the "Students" element
            /*XmlElement element = doc.CreateElement("Students");
            root.AppendChild(element);*/
            for (i = 0; i < dataGridView2.RowCount; i++)
            {
                if (dataGridView2.Rows[i].Cells[dataGridView2.ColumnCount - 1].Value.ToString() =="0")
                {
                    flag = false;
                }
            }

            if(flag)
            {
                for (i = 0; i < dataGridView2.RowCount; i++)
                {
                    XmlNode courses = doc.CreateElement("student");
                    courses.Attributes.Append(CreateAttribute(courses, "id", dataGridView2.Rows[i].Cells[0].Value.ToString()));
                    root.AppendChild(courses);
                    XmlNode task = doc.CreateElement("task");
                    task.Attributes.Append(CreateAttribute(task, "CourseName", course));
                    task.Attributes.Append(CreateAttribute(task, "Task", taskname));
                    task.Attributes.Append(CreateAttribute(task, "Rubric", name));
                    task.Attributes.Append(CreateAttribute(task, "Grade", dataGridView2.Rows[i].Cells[dataGridView2.ColumnCount - 1].Value.ToString()));
                    courses.AppendChild(task);
                }
                doc.Save("evaluate.xml");
                MessageBox.Show("XML File created ! ");
            }
            else
            {
                MessageBox.Show("All students should be graded");
            }
               
        }
    }
}
