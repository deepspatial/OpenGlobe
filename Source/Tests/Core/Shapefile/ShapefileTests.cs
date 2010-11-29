﻿#region License
//
// (C) Copyright 2010 Patrick Cozzi and Kevin Ring
//
// Distributed under the Boost Software License, Version 1.0.
// See License.txt or http://www.boost.org/LICENSE_1_0.txt.
//
#endregion

using System.IO;
using NUnit.Framework;

namespace OpenGlobe.Core
{
    [TestFixture]
    public class ShapefileTests
    {
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileNotFound()
        {
            new Shapefile("does.not.exist");
        }

        [Test]
        [Category("RequiresData")]
        public void PointShape()
        {
            Shapefile sf = new Shapefile("amtrakx020.shp");

            //
            // Verify header
            //
            Assert.AreEqual(ShapeType.Point, sf.ShapeType);
            Assert.AreEqual(-124.21199, sf.Extent.LowerLeft.X, 1e-4);
            Assert.AreEqual(24.5559350038636, sf.Extent.LowerLeft.Y, 1e-13);
            Assert.AreEqual(-70.290492990603, sf.Extent.UpperRight.X, 1e-13);
            Assert.AreEqual(50.3315320102401, sf.Extent.UpperRight.Y, 1e-13);

            //
            // Verify records
            //
            Assert.AreEqual(823, sf.Count);

            Vector2D[] positions = new Vector2D[sf.Count];
            for (int i = 0; i < sf.Count; ++i)
            {
                //
                // This first assert would not hold if the shapefile
                // contains null shapes, which are filtered out.
                //
                Assert.AreEqual(i + 1, sf[i].RecordNumber);
                Assert.AreEqual(ShapeType.Point, sf[i].ShapeType);

                positions[i] = ((PointShape)sf[i]).Position;
            }

            int j = 0;
            foreach (Shape shape in sf)
            {
                Assert.AreEqual(positions[j++], ((PointShape)shape).Position);
            }
        }
    }
}
