namespace Visualizer.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Icon ALab
        {
            get
            {
                return (Icon) ResourceManager.GetObject("ALab", resourceCulture);
            }
        }

        internal static Bitmap close_MC
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("close_MC", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap Gear24
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("Gear24", resourceCulture);
            }
        }

        internal static Bitmap MouseBlackBig
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MouseBlackBig", resourceCulture);
            }
        }

        internal static Bitmap MouseGrayBig
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MouseGrayBig", resourceCulture);
            }
        }

        internal static Bitmap MouseLGrayBig
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MouseLGrayBig", resourceCulture);
            }
        }

        internal static Bitmap MouseModeBig
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("MouseModeBig", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Visualizer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

