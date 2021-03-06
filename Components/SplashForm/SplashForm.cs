/*
 * Copyright (c) 2006-2013 Michal Kuncl <michal.kuncl@gmail.com> http://www.pavucina.info
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */


using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ArachNGIN.Components.SplashForm
{
    /// <summary>
    ///     Splash window class
    /// </summary>
    public class SplashForm : Form
    {
        #region Dispose        

        /// <summary>
        ///     Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            _mBmp.Dispose();
            base.Dispose(disposing);
            //_mInstance = null
        }

        #endregion // Dispose

        #region Constructor        

        /// <summary>
        ///     Initializes a new instance of the <see cref="SplashForm" /> class.
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <param name="col">The color.</param>
        /// <exception cref="System.Exception">Failed to load the bitmap file  + imageFile</exception>
        public SplashForm(string imageFile, Color col)
        {
            Debug.Assert(!string.IsNullOrEmpty(imageFile),
                "A valid file path has to be given");
            // ====================================================================================
            // Setup the form
            // ==================================================================================== 
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;

            // make form transparent
            TransparencyKey = BackColor;

            // tie up the events
            KeyUp += SplashFormKeyUp;
            Paint += SplashFormPaint;
            MouseDown += SplashFormMouseClick;

            // load and make the bitmap transparent
            _mBmp = new Bitmap(imageFile);

            if (_mBmp == null)
                throw new Exception("Failed to load the bitmap file " + imageFile);
            _mBmp.MakeTransparent(col);

            // resize the form to the size of the iamge
            Width = _mBmp.Width;
            Height = _mBmp.Height;

            // center the form
            StartPosition = FormStartPosition.CenterScreen;

            // thread handling
            _mDelegateClose = InternalCloseSplash;
        }

        /// <summary>
        ///     Sets background color of splash window
        /// </summary>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        /// </PermissionSet>
        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        #endregion // Constructor

        #region Public methods        

        /// <summary>
        ///     Shows splash window modally.
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <param name="col">The col.</param>
        public static void ShowModal(string imageFile, Color col)
        {
            _mImageFile = imageFile;
            _mTransColor = col;
            MySplashThreadFunc();
        }

        /// <summary>
        ///     Call this method with the image file path and the color in the image to be rendered transparent
        /// </summary>
        /// <param name="imageFile">The image file.</param>
        /// <param name="col">The color.</param>
        public static void StartSplash(string imageFile, Color col)
        {
            _mImageFile = imageFile;
            _mTransColor = col;
            // Create and Start the splash thread
            var instanceCaller = new Thread(MySplashThreadFunc);
            instanceCaller.Start();
        }

        /// <summary>
        ///     Closes the splash.
        /// </summary>
        public static void CloseSplash()
        {
            if (_mInstance != null)
                _mInstance.Invoke(_mInstance._mDelegateClose);
        }

        #endregion // Public methods

        #region Threading code

        // ultimately this is called for closing the splash window        
        private void InternalCloseSplash()
        {
            Close();
            Dispose();
        }

        // this is called by the new thread to show the splash screen
        private static void MySplashThreadFunc()
        {
            _mInstance = new SplashForm(_mImageFile, _mTransColor);
            _mInstance.TopMost = false;
            _mInstance.ShowDialog();
        }

        #endregion // Multithreading code

        #region Event Handlers

        private void SplashFormMouseClick(object sender, MouseEventArgs e)
        {
            InternalCloseSplash();
        }

        private void SplashFormPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_mBmp, 0, 0);
        }

        private void SplashFormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                InternalCloseSplash();
        }

        #endregion // Event Handlers

        #region Private variables

        private static SplashForm _mInstance;
        private static string _mImageFile;
        private static Color _mTransColor;
        private readonly Bitmap _mBmp;
        private readonly DelegateCloseSplash _mDelegateClose;

        #endregion
    }
}