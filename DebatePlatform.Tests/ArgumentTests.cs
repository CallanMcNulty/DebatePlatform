using DebatePlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DebatePlatform.Tests
{
    public class ArgumentTests
    {
        [Fact]
        public void CanParentTest()
        {
            //Arrange
            Argument arg = new Argument();
            arg.ArgumentId = 1;
            Argument arg2 = new Argument();
            arg2.ArgumentId = 2;
            //Act
            arg.Parent = arg2;
            //Assert
            Assert.Equal(2, arg.Parent.ArgumentId);
        }
        [Fact]
        public void GetTotalStrengthMultipleAdditionsTest()
        {
            //Arrange
            Argument arg1 = new Argument();
            arg1.ArgumentId = 1;
            arg1.Strength = 1;
            arg1.IsAffirmative = true;
            Argument arg2 = new Argument();
            arg2.ArgumentId = 2;
            arg2.Strength = 1;
            arg2.IsAffirmative = true;
            Argument arg3 = new Argument();
            arg3.ArgumentId = 3;
            arg3.Strength = 1;
            arg3.IsAffirmative = true;
            arg2.Parent = arg1;
            arg1.Children = new List<Argument>() { arg2 };
            arg3.Parent = arg2;
            arg2.Children = new List<Argument>() { arg3 };
            arg3.Children = new List<Argument>();
            //Act
            int result = arg1.GetTotalStrength();
            //Assert
            Assert.Equal(3, result);
        }
        [Fact]
        public void GetTotalStrengthSubtractionTest()
        {
            //Arrange
            Argument arg1 = new Argument();
            arg1.ArgumentId = 1;
            arg1.Strength = 1;
            arg1.IsAffirmative = true;
            Argument arg2 = new Argument();
            arg2.ArgumentId = 2;
            arg2.Strength = 1;
            arg2.IsAffirmative = false;
            arg2.Parent = arg1;
            arg1.Children = new List<Argument>() { arg2 };
            arg2.Children = new List<Argument>();
            //Act
            int result = arg1.GetTotalStrength();
            //Assert
            Assert.Equal(0, result);
        }
        [Fact]
        public void GetTotalStrengthSubtractionOverflowTest()
        {
            //Arrange
            Argument arg1 = new Argument();
            arg1.ArgumentId = 1;
            arg1.Strength = 1;
            arg1.IsAffirmative = true;
            arg1.ParentId = 10;
            Argument arg2 = new Argument();
            arg2.ArgumentId = 2;
            arg2.Strength = 5;
            arg2.IsAffirmative = false;
            arg2.Parent = arg1;
            arg1.Children = new List<Argument>() { arg2 };
            arg2.Children = new List<Argument>();
            //Act
            int result = arg1.GetTotalStrength();
            //Assert
            Assert.Equal(0, result);
        }
        [Fact]
        public void GetTotalStrengthSubtractionFromSupportTest()
        {
            //Arrange
            Argument arg1 = new Argument();
            arg1.ArgumentId = 1;
            arg1.Strength = 1;
            arg1.IsAffirmative = true;
            Argument arg2 = new Argument();
            arg2.ArgumentId = 2;
            arg2.Strength = 1;
            arg2.IsAffirmative = true;
            Argument arg3 = new Argument();
            arg3.ArgumentId = 3;
            arg3.Strength = 1;
            arg3.IsAffirmative = false;
            arg2.Parent = arg1;
            arg1.Children = new List<Argument>() { arg2 };
            arg3.Parent = arg2;
            arg2.Children = new List<Argument>() { arg3 };
            arg3.Children = new List<Argument>();
            //Act
            int result = arg1.GetTotalStrength();
            //Assert
            Assert.Equal(1, result);
        }
    }
}

