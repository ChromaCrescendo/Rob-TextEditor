﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TextEditorCSharp
{
    public partial class Form1 : Form
    {        
        //make a string array in case we want to use the file data as an array (unused)
        //make a string for the file path, to be utilized in any function
        string[] fileAsArray; string filePath; string chosenColor; float fontSize; string appName = "Let's Get Textual! Text Editor";   

        public static class Prompt
        {
            //method show dialog where you can customize values of text and caption to be displayed
            public static string ShowDialog(string text, string caption)
            {
                //Constructor for a new form where we are showing everything
                Form prompt = new Form()
                {
                    //Alternate form of class attribute assignment
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
                //Shortcut if-then statement to check if user pressed ok
                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }

            /*public static string ShowList(string text, string caption)
            {
                //Constructor for a new form where we are showing everything
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                ListBox listBox = new ListBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(listBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
                //Shortcut if-then statement to check if user pressed ok
                return prompt.ShowDialog() == DialogResult.OK ? listBox.Text : "";
            }*/
        }

        public Form1()
        {
            //Initialize form and set word wrap, and black font color, as checked by default
            InitializeComponent();
            wordWrapToolStripMenuItem.Checked = true;
        }
        
        private void tb_TextChanged(object sender, EventArgs e)
        {

        }        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create new instance of class OpenFileDialog and set it's paramaters
            OpenFileDialog openfile = new OpenFileDialog();
            //Set filter to only allow users to open text files
            openfile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == openfile.ShowDialog())
            {
                //if user clicks ok try to open file
                try
                {
                    //set file path as string equal to file name of the instantiated class
                    filePath = openfile.FileName;
                    //read all text from file path as a single string
                    //and write it to the text box
                    rtb.Text = File.ReadAllText(filePath);
                    //save the file data as an array (unused)
                    fileAsArray = File.ReadAllLines(filePath);
                    //show where we opened the file from
                    MessageBox.Show("File Opened From: " + filePath);
                    //Show file name on title bar
                    //Also keep application name on title bar
                    Form1.ActiveForm.Text = String.Format("({0}) {1}", filePath, appName);
                }
                //if anything goes wrong, catch the error, prevent a crash
                catch(Exception error)
                {
                    //tell the user the exact error message
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //write text in text box to previously chosen file path
                File.WriteAllText(filePath, rtb.Text);
                //tell user where we wrote the file
                MessageBox.Show("File Written to: " + filePath);
            }
            //attempt to guess extremely specific error exception
            //just in case, tell user exact error
            //works both ways :)
            catch(Exception error)
            {
                MessageBox.Show("[Can't save to unknown file path, try 'Save As']\n" + error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //same as before but with save instead of open
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == savefile.ShowDialog())
            {
                //try to save file at path chosen in save file dialog
                try
                {
                    filePath = savefile.FileName;
                    File.WriteAllText(filePath, rtb.Text);
                    MessageBox.Show("File Written to: " + filePath);
                    Form1.ActiveForm.Text = String.Format("({0}) {1}", filePath, appName);
                }
                //throw error message for every exception
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findWhat = Prompt.ShowDialog("Find what?", "Find");
            int findIndex = rtb.Text.IndexOf(findWhat);
            MessageBox.Show("word found at index " + findIndex);
        }

        private void andReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Instantiate two new prompt classes to grab find and replace values
                string replaceWhat = Prompt.ShowDialog("Replace what?", "Find & Replace");
                string withWhat = Prompt.ShowDialog("With what?", "Find & Replace");
                //Grab the current text as a string and make a new string replacing
                //every instance of the first user given value with the second
                string grabText = rtb.Text;
                string newText = grabText.Replace(replaceWhat, withWhat);
                //Change the text to its new value
                rtb.Text = newText;
            }
            catch(ArgumentException error)
            {
                MessageBox.Show("You failed to provide text to replace!\n" + error.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if word wrap is checked
            if(wordWrapToolStripMenuItem.Checked == true)
            {
                //uncheck if previously checked and set word wrap off
                wordWrapToolStripMenuItem.Checked = false;
                rtb.WordWrap = false;
            }
            else
            {
                //check if previously unchecked and set word wrap on
                wordWrapToolStripMenuItem.Checked = true;
                rtb.WordWrap = true;
            }
        }      

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(appName + " is a unique and fun text editor for editing and saving your text files on the fly!\n" +
                "Original Author: Robert Tripp Ross IV\n" +
                "V0.2\n" +
                "Course Name: Web Development And Coding Bootcamp");
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chosenColor = Prompt.ShowDialog("Pick a color:", "Change Font Color");
            rtb.ForeColor = Color.FromName(chosenColor);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //whenever you resize a form set the text box to match the width, and the height to
            //match the height of the form minus the height of the menustrip
            rtb.Width = Form1.ActiveForm.Width - 16;
            rtb.Height = Form1.ActiveForm.Height - menuStrip1.Height - 42;
        }

        private void windowThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string fontSizeString = Prompt.ShowDialog("Enter a size:", "Font Size Selection");
                fontSize = float.Parse(fontSizeString);
                rtb.Font = new Font(rtb.Font.FontFamily, fontSize, rtb.Font.Style);
            }            
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }     
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Bold);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Underline);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Italic);
        }
    }
}
