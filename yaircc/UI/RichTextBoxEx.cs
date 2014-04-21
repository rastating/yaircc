using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

/// <summary>
/// Represents a standard <see cref="RichTextBox"/> with some
/// minor added functionality.
/// </summary>
/// <remarks>
/// AdvRichTextBox provides methods to maintain performance
/// while it is being updated. Additional formatting features
/// have also been added.
/// </remarks>
public class RichTextBoxEx : RichTextBox
{
    private const int CFM_UNDERLINETYPE = 8388608;
    private const int EM_SETCHARFORMAT = 1092;
    private const int EM_GETCHARFORMAT = 1082;

    [StructLayout(LayoutKind.Sequential)]
    private struct CHARFORMAT
    {
        public int cbSize;
        public uint dwMask;
        public uint dwEffects;
        public int yHeight;
        public int yOffset;
        public int crTextColor;
        public byte bCharSet;
        public byte bPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] szFaceName;

        // CHARFORMAT2 from here onwards.
        public short wWeight;
        public short sSpacing;
        public int crBackColor;
        public int LCID;
        public uint dwReserved;
        public short sStyle;
        public short wKerning;
        public byte bUnderlineType;
        public byte bAnimation;
        public byte bRevAuthor;
    }

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(HandleRef hWnd, int msg,
                                           int wParam, ref CHARFORMAT lp);


    /// <summary>
    /// Gets or sets the underline style to apply to the
    /// current selection or insertion point.
    /// </summary>
    /// <remarks>
    /// Underline styles can be set to any value of the
    /// <see cref="UnderlineStyle"/> enumeration.
    /// </remarks>
    public UnderlineStyle SelectionUnderlineStyle
    {
        get
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);

            // Get the underline style.
            SendMessage(new HandleRef(this, Handle), EM_GETCHARFORMAT,
                         SCF_SELECTION, ref fmt);

            // Default to no underline.
            if ((fmt.dwMask & CFM_UNDERLINETYPE) == 0)
                return UnderlineStyle.None;

            byte style = (byte)(fmt.bUnderlineType & 0x0F);

            return (UnderlineStyle)style;
        }

        set
        {
            // Ensure we don't alter the color by accident.
            UnderlineColor color = SelectionUnderlineColor;

            // Ensure we don't show it if it shouldn't be shown.
            if (value == UnderlineStyle.None)
                color = UnderlineColor.Black;

            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_UNDERLINETYPE;
            fmt.bUnderlineType = (byte)((byte)value | (byte)color);

            // Set the underline type.
            SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT,
                         SCF_SELECTION, ref fmt);
        }
    }

    /// <summary>
    /// Specifies the style of underline that should be
    /// applied to the text.
    /// </summary>
    public enum UnderlineStyle
    {
        /// <summary>
        /// No underlining.
        /// </summary>
        None = 0,

        /// <summary>
        /// Standard underlining across all words.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Standard underlining broken between words.
        /// </summary>
        Word = 2,

        /// <summary>
        /// Double line underlining.
        /// </summary>
        Double = 3,

        /// <summary>
        /// Dotted underlining.
        /// </summary>
        Dotted = 4,

        /// <summary>
        /// Dashed underlining.
        /// </summary>
        Dash = 5,

        /// <summary>
        /// Dash-dot ("-.-.") underlining.
        /// </summary>
        DashDot = 6,

        /// <summary>
        /// Dash-dot-dot ("-..-..") underlining.
        /// </summary>
        DashDotDot = 7,

        /// <summary>
        /// Wave underlining (like spelling mistakes in MS Word).
        /// </summary>
        Wave = 8,

        /// <summary>
        /// Extra thick standard underlining.
        /// </summary>
        Thick = 9,

        /// <summary>
        /// Extra thin standard underlining.
        /// </summary>
        HairLine = 10,

        /// <summary>
        /// Double thickness wave underlining.
        /// </summary>
        DoubleWave = 11,

        /// <summary>
        /// Thick wave underlining.
        /// </summary>
        HeavyWave = 12,

        /// <summary>
        /// Extra long dash underlining.
        /// </summary>
        LongDash = 13
    }

    /// <summary>
    /// Gets or sets the underline color to apply to the
    /// current selection or insertion point.
    /// </summary>
    /// <remarks>
    /// Underline colors can be set to any value of the
    /// <see cref="UnderlineColor"/> enumeration.
    /// </remarks>
    public UnderlineColor SelectionUnderlineColor
    {
        get
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);

            // Get the underline color.
            SendMessage(new HandleRef(this, Handle), EM_GETCHARFORMAT,
                         SCF_SELECTION, ref fmt);

            // Default to black.
            if ((fmt.dwMask & CFM_UNDERLINETYPE) == 0)
                return UnderlineColor.Black;

            byte style = (byte)(fmt.bUnderlineType & 0xF0);

            return (UnderlineColor)style;
        }

        set
        {
            // Ensure we don't alter the style.
            UnderlineStyle style = SelectionUnderlineStyle;

            // Ensure we don't show it if it shouldn't be shown.
            if (style == UnderlineStyle.None)
                value = UnderlineColor.Black;

            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_UNDERLINETYPE;
            fmt.bUnderlineType = (byte)((byte)style | (byte)value);

            // Set the underline color.
            SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT,
                         SCF_SELECTION, ref fmt);
        }
    }

    /// <summary>
    /// Specifies the color of underline that should be
    /// applied to the text.
    /// </summary>
    /// <remarks>
    /// I named these colors by their appearance, so some
    /// of them might not be what you expect. Please email
    /// me if you feel one should be changed.
    /// </remarks>
    public enum UnderlineColor
    {
        /// <summary>Black.</summary>
        Black = 0x00,

        /// <summary>Blue.</summary>
        Blue = 0x10,

        /// <summary>Cyan.</summary>
        Cyan = 0x20,

        /// <summary>Lime green.</summary>
        LimeGreen = 0x30,

        /// <summary>Magenta.</summary>
        Magenta = 0x40,

        /// <summary>Red.</summary>
        Red = 0x50,

        /// <summary>Yellow.</summary>
        Yellow = 0x60,

        /// <summary>White.</summary>
        White = 0x70,

        /// <summary>DarkBlue.</summary>
        DarkBlue = 0x80,

        /// <summary>DarkCyan.</summary>
        DarkCyan = 0x90,

        /// <summary>Green.</summary>
        Green = 0xA0,

        /// <summary>Dark magenta.</summary>
        DarkMagenta = 0xB0,

        /// <summary>Brown.</summary>
        Brown = 0xC0,

        /// <summary>Olive green.</summary>
        OliveGreen = 0xD0,

        /// <summary>Dark gray.</summary>
        DarkGray = 0xE0,

        /// <summary>Gray.</summary>
        Gray = 0xF0
    }

    /// <summary>
    /// Maintains performance while updating.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is recommended to call this method before doing
    /// any major updates that you do not wish the user to
    /// see. Remember to call EndUpdate when you are finished
    /// with the update. Nested calls are supported.
    /// </para>
    /// <para>
    /// Calling this method will prevent redrawing. It will
    /// also setup the event mask of the underlying richedit
    /// control so that no events are sent.
    /// </para>
    /// </remarks>
    public void BeginUpdate()
    {
        // Deal with nested calls.
        ++updating;

        if (updating > 1)
            return;

        // Prevent the control from raising any events.
        oldEventMask = SendMessage(new HandleRef(this, Handle),
                                    EM_SETEVENTMASK, 0, 0);

        // Prevent the control from redrawing itself.
        SendMessage(new HandleRef(this, Handle),
                     WM_SETREDRAW, 0, 0);
    }

    /// <summary>
    /// Resumes drawing and event handling.
    /// </summary>
    /// <remarks>
    /// This method should be called every time a call is made
    /// made to BeginUpdate. It resets the event mask to it's
    /// original value and enables redrawing of the control.
    /// </remarks>
    public void EndUpdate()
    {
        // Deal with nested calls.
        --updating;

        if (updating > 0)
            return;

        // Allow the control to redraw itself.
        SendMessage(new HandleRef(this, Handle),
                     WM_SETREDRAW, 1, 0);

        // Allow the control to raise event messages.
        SendMessage(new HandleRef(this, Handle),
                     EM_SETEVENTMASK, 0, oldEventMask);
    }

    /// <summary>
    /// Gets or sets the alignment to apply to the current
    /// selection or insertion point.
    /// </summary>
    /// <remarks>
    /// Replaces the SelectionAlignment from
    /// <see cref="RichTextBox"/>.
    /// </remarks>
    public new TextAlign SelectionAlignment
    {
        get
        {
            PARAFORMAT fmt = new PARAFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);

            // Get the alignment.
            SendMessage(new HandleRef(this, Handle),
                         EM_GETPARAFORMAT,
                         SCF_SELECTION, ref fmt);

            // Default to Left align.
            if ((fmt.dwMask & PFM_ALIGNMENT) == 0)
                return TextAlign.Left;

            return (TextAlign)fmt.wAlignment;
        }

        set
        {
            PARAFORMAT fmt = new PARAFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = PFM_ALIGNMENT;
            fmt.wAlignment = (short)value;

            // Set the alignment.
            SendMessage(new HandleRef(this, Handle),
                         EM_SETPARAFORMAT,
                         SCF_SELECTION, ref fmt);
        }
    }

    /// <summary>
    /// This member overrides
    /// <see cref="Control"/>.OnHandleCreated.
    /// </summary>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        // Enable support for justification.
        SendMessage(new HandleRef(this, Handle),
                     EM_SETTYPOGRAPHYOPTIONS,
                     TO_ADVANCEDTYPOGRAPHY,
                     TO_ADVANCEDTYPOGRAPHY);
    }

    private int updating = 0;
    private int oldEventMask = 0;

    // Constants from the Platform SDK.
    private const int EM_SETEVENTMASK = 1073;
    private const int EM_GETPARAFORMAT = 1085;
    private const int EM_SETPARAFORMAT = 1095;
    private const int EM_SETTYPOGRAPHYOPTIONS = 1226;
    private const int WM_SETREDRAW = 11;
    private const int TO_ADVANCEDTYPOGRAPHY = 1;
    private const int PFM_ALIGNMENT = 8;
    private const int SCF_SELECTION = 1;

    // It makes no difference if we use PARAFORMAT or
    // PARAFORMAT2 here, so I have opted for PARAFORMAT2.
    [StructLayout(LayoutKind.Sequential)]
    private struct PARAFORMAT
    {
        public int cbSize;
        public uint dwMask;
        public short wNumbering;
        public short wReserved;
        public int dxStartIndent;
        public int dxRightIndent;
        public int dxOffset;
        public short wAlignment;
        public short cTabCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] rgxTabs;

        // PARAFORMAT2 from here onwards.
        public int dySpaceBefore;
        public int dySpaceAfter;
        public int dyLineSpacing;
        public short sStyle;
        public byte bLineSpacingRule;
        public byte bOutlineLevel;
        public short wShadingWeight;
        public short wShadingStyle;
        public short wNumberingStart;
        public short wNumberingStyle;
        public short wNumberingTab;
        public short wBorderSpace;
        public short wBorderWidth;
        public short wBorders;
    }

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(HandleRef hWnd,
                                           int msg,
                                           int wParam,
                                           int lParam);

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern int SendMessage(HandleRef hWnd,
                                           int msg,
                                           int wParam,
                                           ref PARAFORMAT lp);

    private const int CFM_BACKCOLOR = 67108864;

    /// <summary>
    /// Gets or sets the background color to apply to the
    /// current selection or insertion point.
    /// </summary>
    /// <remarks>
    /// If the selection contains more than one background
    /// color, then this property will indicate it by
    /// returning Color.Empty.
    /// </remarks>
    public Color SelectionBackColor
    {
        get
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);

            // Get the background color.
            SendMessage(new HandleRef(this, Handle), EM_GETCHARFORMAT,
                         SCF_SELECTION, ref fmt);

            // Default to Color.Empty as there could be
            // several colors present in this selection.
            if ((fmt.dwMask & CFM_BACKCOLOR) == 0)
                return Color.Empty;

            // Deal with the weird Windows color format.
            int backCol = fmt.crBackColor;
            Color ret = ColorTranslator.FromWin32(backCol);

            return ret;
        }

        set
        {
            CHARFORMAT fmt = new CHARFORMAT();
            fmt.cbSize = Marshal.SizeOf(fmt);
            fmt.dwMask = CFM_BACKCOLOR;

            // Deal with the weird Windows color format.
            fmt.crBackColor = ColorTranslator.ToWin32(value);

            // Set the background color.
            SendMessage(new HandleRef(this, Handle), EM_SETCHARFORMAT,
                         SCF_SELECTION, ref fmt);
        }
    }
}

/// <summary>
/// Specifies how text in a <see cref="RichTextBoxEx"/> is
/// horizontally aligned.
/// </summary>
public enum TextAlign
{
    /// <summary>
    /// The text is aligned to the left.
    /// </summary>
    Left = 1,

    /// <summary>
    /// The text is aligned to the right.
    /// </summary>
    Right = 2,

    /// <summary>
    /// The text is aligned in the center.
    /// </summary>
    Center = 3,

    /// <summary>
    /// The text is justified.
    /// </summary>
    Justify = 4
}
