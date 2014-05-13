using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Squid;
using System;
using System.Reflection;
using Cursor = Squid.Cursor;

public class SampleGui01 : GuiRenderer
{
    // simple object model for ListView
    public class MyData
    {
        public string Name;
        public DateTime Date;
        public int Rating;
    }

    void Start()
    {
        if (CustomSkin == null || Skin == null)
            CreateSkin();

        GuiHost.SetSkin(Skin);

        // create auto-atlas
        // note: size parameter will go away next version (autom. size)
        Texture2D atlas = CreateAtlas(Skin, "guiAtlas01", 2, 1024) as Texture2D;

        // optional: save the returned texture as png
        //var bytes = atlas.EncodeToPNG();
        //System.IO.File.WriteAllBytes("F:\\atlas.png", bytes);

        if (Layout == null)
            CreateGui();

        GuiHost.GlobalFadeSpeed = 250;
    }

    void CreateSkin()
    {
        ControlStyle baseStyle = new ControlStyle();
        baseStyle.Tiling = TextureMode.Grid;
        baseStyle.Grid = new Margin(3);
        baseStyle.Texture = "button";
        baseStyle.Tint = ColorInt.ARGB(1, .8f, .9f, 1f);
        baseStyle.Default.Tint = ColorInt.ARGB(.6f, 1f, 1f, 1f);

        ControlStyle titlbar = new ControlStyle();
        titlbar.Tiling = TextureMode.Grid;
        titlbar.Grid = new Margin(6);
        titlbar.Texture = "button";
        //titlbar.Tint = ColorInt.RGBA(1f, .8f, .6f, 1);
        //titlbar.Tint = ColorInt.RGBA(.6f, .8f, 1f, 1);
        titlbar.TextPadding = new Margin(8, 0, 8, 0);
        titlbar.TextAlign = Alignment.MiddleCenter;

        ControlStyle buttonStyle = new ControlStyle(baseStyle);
        buttonStyle.TextPadding = new Margin(8, 0, 8, 0);
        //buttonStyle.TextPadding = new Margin(0);
        buttonStyle.TextAlign = Alignment.MiddleCenter;

        ControlStyle tabStyle = new ControlStyle(baseStyle);
        tabStyle.TextPadding = new Margin(8, 0, 8, 0);
        tabStyle.TextAlign = Alignment.MiddleLeft;
        //tabStyle.Texture = "tab";

        ControlStyle windowStyle = new ControlStyle();
        windowStyle.Tiling = TextureMode.Grid;
        windowStyle.Grid = new Margin(9);
        windowStyle.Texture = "window";

        ControlStyle tooltipStyle = new ControlStyle(windowStyle);
        tooltipStyle.TextPadding = new Margin(8);
        tooltipStyle.TextAlign = Alignment.TopLeft;

        ControlStyle frameStyle = new ControlStyle();
        frameStyle.Tiling = TextureMode.Grid;
        frameStyle.Grid = new Margin(6);
        frameStyle.Texture = "frame";

        ControlStyle insetStyle = new ControlStyle();
        insetStyle.Tiling = TextureMode.Grid;
        insetStyle.Grid = new Margin(6);
        insetStyle.Texture = "inset";

        ControlStyle textboxStyle = new ControlStyle(insetStyle);
        textboxStyle.TextColor = ColorInt.ARGB(1, .5f, .5f, .5f);
        textboxStyle.TextPadding = new Margin(8, 0, 8, 0);
        textboxStyle.Texture = "textbox";

        ControlStyle scrollbarStyle = new ControlStyle();
        scrollbarStyle.Tiling = TextureMode.Grid;
        scrollbarStyle.Texture = "scrollbar";

        ControlStyle multilineStyle = new ControlStyle();
        multilineStyle.TextAlign = Alignment.TopLeft;
        multilineStyle.TextPadding = new Margin(8);
        multilineStyle.TextColor = ColorInt.ARGB(1, .5f, .5f, .5f);

        ControlStyle labelStyle = new ControlStyle();
        labelStyle.TextPadding = new Margin(8, 0, 8, 0);
        labelStyle.TextAlign = Alignment.MiddleLeft;
        labelStyle.TextColor = ColorInt.ARGB(.8f, .8f, .8f, 1);
        labelStyle.Default.TextColor = ColorInt.ARGB(1, .5f, .5f, .5f);
        labelStyle.Default.BackColor = 0;

        ControlStyle itemStyle = new ControlStyle(buttonStyle);
        itemStyle.Texture = "item";
        itemStyle.TextPadding = new Margin(8, 0, 8, 0);
        itemStyle.TextAlign = Alignment.MiddleLeft;
        itemStyle.TextColor = ColorInt.ARGB(1, .8f, .8f, .8f);
        itemStyle.Default.Texture = string.Empty;
        itemStyle.Default.Tint = ColorInt.ARGB(.2f, 1f, 1f, 1f);
        itemStyle.Default.TextColor = ColorInt.ARGB(1f, .5f, .5f, .5f);

        Skin = new Squid.Skin();
        Skin.Styles.Add("titlebar", titlbar);
        Skin.Styles.Add("tab", tabStyle);
        Skin.Styles.Add("label", labelStyle);
        Skin.Styles.Add("item", itemStyle);
        Skin.Styles.Add("textbox", textboxStyle);
        Skin.Styles.Add("button", buttonStyle);
        Skin.Styles.Add("window", windowStyle);
        Skin.Styles.Add("inset", insetStyle);
        Skin.Styles.Add("frame", frameStyle);
        Skin.Styles.Add("checkBox", buttonStyle);
        Skin.Styles.Add("comboLabel", labelStyle);
        Skin.Styles.Add("comboButton", buttonStyle);
        Skin.Styles.Add("scrollbar", scrollbarStyle);
        //Skin.Styles.Add("vscrollTrack", buttonStyle);
        Skin.Styles.Add("vscrollButton", buttonStyle);
        Skin.Styles.Add("vscrollUp", buttonStyle);
        //Skin.Styles.Add("hscrollTrack", buttonStyle);
        Skin.Styles.Add("hscrollButton", buttonStyle);
        Skin.Styles.Add("hscrollUp", buttonStyle);
        Skin.Styles.Add("multiline", multilineStyle);
        Skin.Styles.Add("tooltip", tooltipStyle);

        #region cursors

        Point cursorSize = new Point(32, 32);
        Point halfSize = cursorSize / 2;

        Skin.Cursors.Add(Cursors.Default, new Cursor { Texture = "Cursors/Arrow", Size = cursorSize, HotSpot = Point.Zero });
        Skin.Cursors.Add(Cursors.Link, new Cursor { Texture = "Cursors/Link", Size = cursorSize, HotSpot = Point.Zero });
        Skin.Cursors.Add(Cursors.Move, new Cursor { Texture = "Cursors/Move", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.Select, new Cursor { Texture = "Cursors/Select", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.SizeNS, new Cursor { Texture = "Cursors/SizeNS", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.SizeWE, new Cursor { Texture = "Cursors/SizeWE", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.HSplit, new Cursor { Texture = "Cursors/SizeNS", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.VSplit, new Cursor { Texture = "Cursors/SizeWE", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.SizeNESW, new Cursor { Texture = "Cursors/SizeNESW", Size = cursorSize, HotSpot = halfSize });
        Skin.Cursors.Add(Cursors.SizeNWSE, new Cursor { Texture = "Cursors/SizeNWSE", Size = cursorSize, HotSpot = halfSize });

        #endregion
    }

    void CreateGui()
    {
        #region sample window 1 - Anchoring, DropDown, Modal Dialog

        SampleWindow window1 = new SampleWindow();
        window1.Size = new Squid.Point(440, 340);
        window1.Position = new Squid.Point(40, 40);
        window1.Titlebar.Text = "Anchoring, [color=ffffff00]DropDown, Modal Dialog[/color]";
        window1.Resizable = true;
        window1.Parent = Desktop;

        Label label1 = new Label();
        label1.Text = "username:";
        label1.Size = new Squid.Point(122, 32);
        label1.Position = new Squid.Point(60, 100);
        label1.Parent = window1;
        label1.MousePress += label1_OnMouseDown;

        TextBox textbox1 = new TextBox { Name = "textbox" };
        textbox1.Text = "username";
        textbox1.Size = new Squid.Point(222, 32);
        textbox1.Position = new Squid.Point(180, 100);
        textbox1.Style = "textbox";
        textbox1.Parent = window1;
        textbox1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        textbox1.KeyDown += textbox1_KeyDown;

        Label label2 = new Label();
        label2.Text = "password:";
        label2.Size = new Squid.Point(122, 32);
        label2.Position = new Squid.Point(60, 140);
        label2.Parent = window1;
       
        TextBox textbox2 = new TextBox { Name = "textbox" };
        textbox2.PasswordChar = char.Parse("*");
        textbox2.Text = "password";
        textbox2.Size = new Squid.Point(222, 32);
        textbox2.Position = new Squid.Point(180, 140);
        textbox2.Style = "textbox";
        textbox2.Parent = window1;
        textbox2.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

        Button button = new Button();
        button.Size = new Squid.Point(157, 32);
        button.Position = new Squid.Point(437 - 192, 346 - 52);
        button.Text = "login";
        button.Style = "button";
        button.Parent = window1;
        button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        button.Cursor = Cursors.Link;
        button.MouseClick += button_OnMouseClick;

        DropDownList combo = new DropDownList();
        combo.Size = new Squid.Point(222, 32);
        combo.Position = new Squid.Point(180, 180);
        combo.Parent = window1;
        combo.Label.Style = "textbox";
        combo.Button.Style = "button";
        combo.Dropdown.Style = "window";
        combo.Listbox.Margin = new Margin(0, 6, 0, 0);
        //combo.Listbox.Style = "window";
        combo.Listbox.ClipFrame.Margin = new Margin(8, 8, 8, 8);
        combo.Listbox.Scrollbar.Margin = new Margin(0, 4, 4, 4);
        combo.Listbox.Scrollbar.Size = new Squid.Point(14, 10);
        combo.Listbox.Scrollbar.ButtonUp.Style = "vscrollUp";
        combo.Listbox.Scrollbar.ButtonUp.Size = new Squid.Point(10, 20);
        combo.Listbox.Scrollbar.ButtonDown.Style = "vscrollUp";
        combo.Listbox.Scrollbar.ButtonDown.Size = new Squid.Point(10, 20);
        combo.Listbox.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
        combo.Listbox.Scrollbar.Slider.Button.Style = "vscrollButton";
        combo.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        combo.Listbox.Scrollbar.Style = "scrollbar";

        for (int i = 0; i < 10; i++)
        {
            ListBoxItem item = new ListBoxItem();
            item.Text = "listboxitem";
            item.Size = new Squid.Point(100, 32);
            item.Margin = new Margin(0, 0, 0, 4);
            item.Style = "item";

            //if (i == 3)
            //    item.Selected = true;

            combo.Items.Add(item);

            // if (i == 3)
            //     combo.SelectedItem = item;
            if (i == 3)
                item.Selected = true;
        }

        CheckBox box = new CheckBox();
        box.Size = new Squid.Point(157, 26);
        box.Position = new Squid.Point(180, 220);
        box.Text = "remember me";
        box.Parent = window1;
        box.Button.Style = "checkBox";
        box.Button.Size = new Squid.Point(26, 26);
        box.Button.Cursor = Cursors.Link;

        #endregion

        //return;

        #region sample window 2 - SplitContainer, TreeView, ListBox

        SampleWindow window2 = new SampleWindow();
        window2.Size = new Squid.Point(440, 340);
        window2.Position = new Squid.Point(500, 40);
        window2.Titlebar.Text = "SplitContainer, TreeView, ListBox";
        window2.Resizable = true;
        window2.Parent = Desktop;

        SplitContainer split = new SplitContainer();
        split.Dock = DockStyle.Fill;
        split.Parent = window2;
        split.SplitButton.Style = "button";
        //split.Style = "inset";
        split.SplitFrame1.Size = new Point(10, 10);
        split.SplitFrame2.Size = new Point(30, 10);
        split.SplitFrame1.Margin = new Margin(4);
        split.SplitFrame2.Margin = new Margin(4);
        split.SplitButton.Style = "frame";
        //split.SplitFrame1.Style = "inset";
        //split.SplitFrame2.Style = "inset";

        ListBox listbox1 = new ListBox();
        listbox1.Margin = new Squid.Margin(2);
        listbox1.Dock = DockStyle.Fill;
        listbox1.Scrollbar.Style = "scrollbar";
        listbox1.Scrollbar.Size = new Squid.Point(14, 10);
        listbox1.Scrollbar.Slider.Style = "vscrollTrack";
        listbox1.Scrollbar.Slider.Button.Style = "vscrollButton";
        listbox1.Scrollbar.ButtonUp.Style = "vscrollUp";
        listbox1.Scrollbar.ButtonUp.Size = new Squid.Point(10, 20);
        listbox1.Scrollbar.ButtonDown.Style = "vscrollUp";
        listbox1.Scrollbar.ButtonDown.Size = new Squid.Point(10, 20);
        listbox1.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
        listbox1.Multiselect = true;
        listbox1.MaxSelected = 4;
        listbox1.Parent = split.SplitFrame2;
        listbox1.Margin = new Margin(2);

        for (int i = 0; i < 30; i++)
        {
            ListBoxItem item = new ListBoxItem();
            item.Text = "listboxitem";
            item.Size = new Squid.Point(100, 32);
            item.Margin = new Margin(0, 0, 0, 1);
            item.Style = "item";
            item.Tooltip = "This is a multine tooltip.\nThe second line begins here.\n[color=ff00ee55]The third line is even colored.[/color]";
            listbox1.Items.Add(item);
        }

        TreeView treeview = new TreeView();
        treeview.Dock = DockStyle.Fill;
        treeview.Margin = new Squid.Margin(2);
        treeview.Parent = split.SplitFrame1;
        treeview.Scrollbar.Style = "scrollbar";
        treeview.Scrollbar.Size = new Squid.Point(14, 10);
        treeview.Scrollbar.Slider.Style = "vscrollTrack";
        treeview.Scrollbar.Slider.Button.Style = "vscrollButton";
        treeview.Scrollbar.ButtonUp.Style = "vscrollUp";
        treeview.Scrollbar.ButtonUp.Size = new Squid.Point(10, 20);
        treeview.Scrollbar.ButtonDown.Style = "vscrollUp";
        treeview.Scrollbar.ButtonDown.Size = new Squid.Point(10, 20);
        treeview.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);
        treeview.Indent = 10;
        treeview.Margin = new Margin(2);

        for (int i = 0; i < 30; i++)
        {
            TreeNodeLabel node = new TreeNodeLabel();
            node.Label.Text = "node level 1";
            node.Label.TextAlign = Alignment.MiddleLeft;
            node.Label.Style = "label";
            node.Button.Size = new Point(18, 18);
            node.Button.Margin = new Margin(4, 7, 0, 7);
            node.Size = new Point(100, 32);
            node.Tooltip = node.Label.Text;
            node.Style = "item";
            treeview.Nodes.Add(node);

            for (int i2 = 0; i2 < 3; i2++)
            {
                TreeNodeLabel sub1 = new TreeNodeLabel();
                sub1.Size = new Squid.Point(100, 32);
                sub1.Label.TextAlign = Alignment.MiddleLeft;
                sub1.Label.Style = "label";
                sub1.Button.Size = new Point(16, 16);
                sub1.Button.Margin = new Margin(8);
                sub1.Size = new Point(100, 32);
                sub1.Label.Text = "node level 2";
                sub1.Tooltip = sub1.Label.Text;
                sub1.Style = "item";
                node.Nodes.Add(sub1);

                for (int i3 = 0; i3 < 3; i3++)
                {
                    TreeNodeLabel sub2 = new TreeNodeLabel();
                    sub2.Label.Text = "node level 3";
                    sub2.Label.TextAlign = Alignment.MiddleLeft;
                    sub2.Label.Style = "label";
                    sub2.Button.Size = new Point(16, 16);
                    sub2.Button.Margin = new Margin(8);
                    sub2.Size = new Point(100, 32);
                    sub2.Tooltip = sub2.Label.Text;
                    sub2.Style = "item";
                    sub1.Nodes.Add(sub2);
                }
            }
        }

        #endregion

        #region sample window 3 - Custom Control (Inheritance)

        SampleWindow window3 = new SampleWindow();
        window3.Size = new Point(440, 340);
        window3.Position = new Point(40, 400);
        window3.Resizable = true;
        window3.Titlebar.Text = "Custom Control (Inheritance)";
        window3.Parent = Desktop;

        ChatBox chatbox = new ChatBox();
        chatbox.Dock = DockStyle.Fill;
        window3.Controls.Add(chatbox);

        chatbox.Style = "frame";
        chatbox.Input.Style = "textbox";
        chatbox.Input.Margin = new Margin(8, 0, 8, 8);
        chatbox.Output.Margin = new Margin(8, 8, 8, 8);
        //chatbox.Output.Style = "textbox";
        chatbox.Scrollbar.Margin = new Margin(0, 8, 8, 8);
        chatbox.Scrollbar.Size = new Squid.Point(14, 10);
        chatbox.Scrollbar.Slider.Style = "vscrollTrack";
        chatbox.Scrollbar.Slider.Button.Style = "vscrollButton";
        chatbox.Scrollbar.ButtonUp.Style = "vscrollUp";
        chatbox.Scrollbar.ButtonUp.Size = new Squid.Point(10, 20);
        chatbox.Scrollbar.ButtonDown.Style = "vscrollUp";
        chatbox.Scrollbar.ButtonDown.Size = new Squid.Point(10, 20);
        chatbox.Scrollbar.Slider.Margin = new Margin(0, 2, 0, 2);

        #endregion

        #region sample window 4 - TabControl, TextAlign

        SampleWindow window4 = new SampleWindow();
        window4.Size = new Point(440, 340);
        window4.Position = new Point(500, 400);
        window4.Resizable = true;
        window4.Titlebar.Text = "TabControl, TextAlign";
        window4.Parent = Desktop;

        TabControl tabcontrol = new TabControl();
        tabcontrol.Dock = DockStyle.Fill;
        tabcontrol.Parent = window4;
        tabcontrol.ButtonFrame.Size = new Point(100, 32);
        tabcontrol.Margin = new Margin(0, 8, 0, 0);

        for (int i = 0; i < 6; i++)
        {
            TabPage tabPage = new TabPage();
            //tabPage.Style = "frame";
            tabPage.Margin = new Margin(0, -1, 0, 0);
            tabPage.Button.Style = "tab";
            tabPage.Button.Text = "page" + i;
            tabPage.Button.Tooltip = "Click to change active tab";
            tabPage.Button.Margin = new Margin(0, 0, -1, 0);
            tabcontrol.TabPages.Add(tabPage);

            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.Parent = tabPage;
            lbl.TextWrap = true;
            lbl.Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam\r\n nonumy eirmod tempor invidunt ut labore [color=ff0088ff][url=testurl]click \r\n meh![/url][/color] et dolore magna aliquyam erat, sed diam voluptua.\r\n At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
            lbl.Style = "multiline";
            lbl.BBCodeEnabled = true;
            lbl.LinkClicked += lbl_OnLinkClicked;
        }

        #endregion

        #region sample window 5 - Panel, TextBox

        SampleWindow window5 = new SampleWindow();
        window5.Size = new Point(440, 340);
        window5.Position = new Point(960, 40);
        window5.Resizable = true;
        window5.Titlebar.Text = "Panel, TextBox";
        window5.Parent = Desktop;

        Panel panel = new Panel();
        panel.Style = "frame";
        panel.Dock = DockStyle.Fill;
        panel.Parent = window5;

        panel.ClipFrame.Margin = new Margin(8);

        panel.VScroll.Margin = new Margin(0, 8, 8, 8);
        panel.VScroll.Size = new Squid.Point(14, 10);
        panel.VScroll.Slider.Style = "vscrollTrack";
        panel.VScroll.Slider.Button.Style = "vscrollButton";
        panel.VScroll.ButtonUp.Style = "vscrollUp";
        panel.VScroll.ButtonUp.Size = new Squid.Point(10, 20);
        panel.VScroll.ButtonDown.Style = "vscrollUp";
        panel.VScroll.ButtonDown.Size = new Squid.Point(10, 20);
        panel.VScroll.Slider.Margin = new Margin(0, 2, 0, 2);

        panel.HScroll.Margin = new Margin(8, 0, 8, 8);
        panel.HScroll.Size = new Squid.Point(10, 14);
        panel.HScroll.Slider.Style = "hscrollTrack";
        panel.HScroll.Slider.Button.Style = "hscrollButton";
        panel.HScroll.ButtonUp.Style = "hscrollUp";
        panel.HScroll.ButtonUp.Size = new Squid.Point(20, 10);
        panel.HScroll.ButtonDown.Style = "hscrollUp";
        panel.HScroll.ButtonDown.Size = new Squid.Point(20, 10);
        panel.HScroll.Slider.Margin = new Margin(2, 0, 2, 0);

        for (int i = 0; i < 10; i++)
        {
            Label label = new Label();
            label.Text = "label control:";
            label.Size = new Point(100, 32);
            label.Position = new Point(10, 10 + 45 * i);
            panel.Content.Controls.Add(label);

            TextBox txt = new TextBox();
            txt.Text = "lorem ipsum";
            txt.Size = new Squid.Point(222, 32);
            txt.Position = new Point(110, 10 + 45 * i);
            txt.Style = "textbox";
            txt.AllowDrop = true;
            txt.TabIndex = 1 + i;
            txt.DragDrop += txt_OnDragDrop;
            txt.GotFocus += txt_OnGotFocus;
            panel.Content.Controls.Add(txt);
        }

        #endregion

        #region sample window 6 - ListView

        SampleWindow window6 = new SampleWindow();
        window6.Size = new Point(440, 340);
        window6.Position = new Point(960, 400);
        window6.Resizable = true;
        window6.Titlebar.Text = "Misc";
        window6.Parent = Desktop;

        System.Random rnd = new System.Random();
        List<MyData> models = new List<MyData>();
        for (int i = 0; i < 32; i++)
        {
            MyData data = new MyData();
            data.Name = rnd.Next().ToString();
            data.Date = DateTime.Now.AddMilliseconds(rnd.Next());
            data.Rating = rnd.Next();
            models.Add(data);
        }

        ListView olv = new ListView();
        olv.Margin = new Margin(0, 8, 0, 0);
        olv.Dock = DockStyle.Fill;
        olv.Panel.VScroll.Style = "scrollbar";
        olv.Columns.Add(new ListView.Column { Text = "Name", Aspect = "Name", Width = 120, MinWidth = 48 });
        olv.Columns.Add(new ListView.Column { Text = "Date", Aspect = "Date", Width = 120, MinWidth = 48 });
        olv.Columns.Add(new ListView.Column { Text = "Rating", Aspect = "Rating", Width = 120, MinWidth = 48 });
        olv.StretchLastColumn = true;
        olv.FullRowSelect = true;

        olv.CreateHeader = delegate(object sender, ListView.FormatHeaderEventArgs args)
        {
            Button header = new Button
            {
                Dock = DockStyle.Fill,
                Text = args.Column.Text,
                AllowDrop = true
            };

            header.MouseClick += delegate(Control snd, MouseEventArgs e)
            {
                if (args.Column.Aspect == "Name")
                    olv.Sort<MyData>((a, b) => a.Name.CompareTo(b.Name));
                else if (args.Column.Aspect == "Date")
                    olv.Sort<MyData>((a, b) => a.Date.CompareTo(b.Date));
                else if (args.Column.Aspect == "Rating")
                    olv.Sort<MyData>((a, b) => a.Rating.CompareTo(b.Rating));
            };

            header.MouseDrag += delegate(Control snd, MouseEventArgs e)
            {
                Label drag = new Label();
                drag.Size = snd.Size;
                drag.Position = snd.Location;
                drag.Style = snd.Style;
                drag.Text = ((Button)snd).Text;

                snd.DoDragDrop(drag);
            };

            header.DragLeave += delegate(Control snd, DragDropEventArgs e) { snd.Tint = -1; };
            header.DragEnter += delegate(Control snd, DragDropEventArgs e)
            {
                if (e.Source is Button)
                {
                    snd.Tint = ColorInt.ARGB(1, 0, 1, 0);
                }
                else
                {
                    snd.Tint = ColorInt.ARGB(1, 1, 0, 0);
                    e.Cancel = true;
                }
            };

            header.DragDrop += delegate(Control snd, DragDropEventArgs e)
            {
                snd.Tint = -1;
            };

            return header;
        };

        olv.CreateCell = delegate(object sender, ListView.FormatCellEventArgs args)
        {
            string text = olv.GetAspectValue(args.Model, args.Column);

            Button cell = new Button
            {
                Size = new Point(26, 26),
                Style = "item",
                Dock = DockStyle.Top,
                Text = text,
                Tooltip = text,
                AllowDrop = true
            };

            cell.DragResponse += delegate(Control snd, DragDropEventArgs e)
            {
                snd.State = ControlState.Hot;
            };

            return cell;
        };

        olv.SetObjects(models);

        window6.Controls.Add(olv);


        #endregion
    }

    void textbox1_KeyDown(Control sender, KeyEventArgs args)
    {
       // Debug.Log(args.Key);
    }	
	
	#region event handler

    void txt_OnGotFocus(Control sender)
    {
        TextBox txt = sender as TextBox;
        txt.Select(0, txt.Text.Length);
    }

	void item_OnMouseLeave(Control sender)
    {
        sender.Animation.Stop();
        sender.Animation.Size(new Point(100, 70), 250);
    }

    void item_OnMouseEnter(Control sender)
    {
        sender.Animation.Stop();
        sender.Animation.Size(new Point(100, 140), 250);
    }

    void b1_OnMouseClick(Control sender, MouseEventArgs e)
    {
        if (e.Button != 0) return;

        ControlStyle style = GuiHost.GetStyle("multiline");
        style.TextAlign = (Alignment)sender.Tag;
    }

    void lbl_OnLinkClicked(string href)
    {
        MessageBox dialog = MessageBox.Show(new Point(300, 200), "Message Box", href, MessageBoxButtons.OKCancel, Desktop);
        dialog.OnResult += dialog_OnDialogResult;
        dialog.Animation.Custom(WalkSquare(dialog));
    }

    private System.Collections.IEnumerator WalkSquare(MessageBox dialog)
    {
        yield return dialog.Animation.Position(new Point(10, 10), 1000);
        yield return dialog.Animation.Position(new Point(1000, 10), 1000);
        yield return dialog.Animation.Position(new Point(1000, 600), 1000);
        yield return dialog.Animation.Position(new Point(10, 600), 1000);
    }

    void label1_OnMouseDown(Control sender, MouseEventArgs e)
    {
        if (e.Button != 0) return;

        Button btn = new Button();
        btn.Size = new Squid.Point(157, 26);
        btn.Text = "remember me";
        btn.Position = sender.Location;
        Desktop.DoDragDrop(btn);
    }

    void txt_OnDragDrop(Control sender, DragDropEventArgs e)
    {
        ((TextBox)sender).Text = ((Button)e.DraggedControl).Text;
    }

    void button_OnMouseClick(Control sender, MouseEventArgs e)
    {
        if (e.Button != 0) return;

        MessageBox dialog = MessageBox.Show(new Point(300, 200), "Message Box", "This is a modal dialog\r\nClick any button to close.", MessageBoxButtons.OKCancel, Desktop);
        dialog.OnResult += dialog_OnDialogResult;
    }

    void dialog_OnDialogResult(Dialog sender, DialogResult result)
    {
        // do something
    }
	
	#endregion
}
	