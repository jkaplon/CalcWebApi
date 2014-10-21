using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebCalc.Controllers
{
    public class CalcOperation
    {
        public string FirstArg { get; set; }
        public string SecondArg { get; set; }
        public string Operator { get; set; }
    }

    public class CalcController : ApiController
    {
        // In order to pass values into controller, I'm using [FromUri] to get parameters from the URI query string.
        // I chose to use the URI query string (as opposed to passing data in body of request) since there are not many arguments and initial testing was easier using query sting only.
        public HttpResponseMessage GetCalcAnswer([FromUri]CalcOperation calcOp)
        {
            try
            {
                // Convert operand arguments to Doubles
                // Double data type seems like a reasonable choice and it's what Math.Pow() expects as input.
                Double firstNum = Convert.ToDouble(calcOp.FirstArg);
                Double secondNum = Convert.ToDouble(calcOp.SecondArg);
                Double answer;

                // Declare message with successful value. Numeric onversion errors from above are caught w/FormatExceptions.
                // Unknown operators handled with default switch clause.
                String msg = "Calculation successful";  

                // Use present-tense verb for math operations to avoid URL-encoding hassles with math symbols.
                switch (calcOp.Operator)
                {
                    case "add":
                        // addition
                        answer = firstNum + secondNum;
                        break;
                    case "subtract":
                        // subtraction
                        answer = firstNum - secondNum;
                        break;
                    case "multiply":
                        answer = firstNum * secondNum;
                        break;
                    case "divide":
                        answer = firstNum / secondNum;
                        break;
                    case "exponentiate":
                        // Doesn't seem right to need a library for this, but '^' is a binary operator in C# (and internet searches agree).
                        answer = Math.Pow(firstNum,  secondNum);
                        break;
                    // Add other types of mathematical operations here (above default case).
                    default:
                        // Return second argument as answer (even though it's not the answer) since user might still want to use it as next argument.
                        answer = secondNum;
                        msg = "Unknown operator";
                        break;
                }

                if (answer != 0 && msg != "Unknown operator")
                {
                    // Check FizzBuzz case first since it's a super-set of other 2 cases, use && (AndAlso) to save a few cycles.
                    if (answer % 3 == 0 && answer % 5 == 0)
                    {
                        msg = "FizzBuzz";
                    }
                    else if (answer % 3 == 0)
                    {
                        msg = "Fizz";
                    }
                    else if (answer % 5 == 0)
                    {
                        msg = "Buzz";
                    }
                }

                // I would probably use ViewBag here in standard MVC app, but this is a WebAPI controller with no model and no concept of ViewBag.
                String answerMsg = Convert.ToString(answer) + ", " + msg;
                return Request.CreateResponse(HttpStatusCode.OK, answerMsg);
            }
            catch (FormatException err)
            {
                // Non-numeric values for FirstArg or SecondArg are caught here.
                // This will be a bad user-experience since the error message isn't helpful and user will need to hit browswer back button to try again.
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, err);
            }
            catch
            {
                // Return some kind of generic error message to display in view.
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
