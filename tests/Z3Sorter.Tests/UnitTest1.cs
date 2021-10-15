using FluentAssertions;
using Microsoft.Z3;
using Xunit;

namespace Z3Sorter.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ExplicitOrder()
        {
            using var context = new Context();
            var aIndex = context.MkIntConst("a_index");
            var bIndex = context.MkIntConst("b_index");
            var cIndex = context.MkIntConst("c_index");

            var goal = context.MkGoal(true);
            goal.Assert(context.MkLt(aIndex, bIndex)); // a -> b
            goal.Assert(context.MkLt(bIndex, cIndex)); // b -> c

            var solver = context.MkSolver();
            foreach (var formula in goal.Formulas)
            {
                solver.Assert(formula);
            }
            var result = solver.Check();
            var aResult = solver.Model.ConstInterp(aIndex);
            var bResult = solver.Model.ConstInterp(bIndex);
            var cResult = solver.Model.ConstInterp(cIndex);

            aResult.ToString().Should().Be("-1");
            bResult.ToString().Should().Be("0");
            cResult.ToString().Should().Be("1");
        }

        [Fact]
        public void ManyBeforeOne()
        {
            using var context = new Context();
            var aIndex = context.MkIntConst("a_index");
            var bIndex = context.MkIntConst("b_index");
            var cIndex = context.MkIntConst("c_index");
            var dIndex = context.MkIntConst("d_index");

            var goal = context.MkGoal(true);
            goal.Assert(context.MkLt(aIndex, dIndex)); // a -> d
            goal.Assert(context.MkLt(bIndex, dIndex)); // b -> d
            goal.Assert(context.MkLt(cIndex, dIndex)); // c -> d

            var solver = context.MkSolver();
            foreach (var formula in goal.Formulas)
            {
                solver.Assert(formula);
            }
            var result = solver.Check();
            var aResult = solver.Model.ConstInterp(aIndex);
            var bResult = solver.Model.ConstInterp(bIndex);
            var cResult = solver.Model.ConstInterp(cIndex);
            var dResult = solver.Model.ConstInterp(dIndex);

            aResult.ToString().Should().Be("-1");
            bResult.ToString().Should().Be("-1");
            cResult.ToString().Should().Be("-1");
            dResult.ToString().Should().Be("0");
        }
    }
}
