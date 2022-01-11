resource "aws_lambda_function" "create_session" {
  function_name = "${local.prefix}-create-session"
  filename      = "create-session.zip"
  handler       = "CreateSession::CreateSession.Function::Handler"
  runtime       = "dotnetcore3.1"
  memory_size   = 256
  timeout       = 30
  role          = aws_iam_role.create_session.arn
}

resource "aws_iam_role" "create_session" {
  name = "${local.prefix}-create-session"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Effect": "Allow",
      "Sid": ""
    }
  ]
}
EOF
}

resource "aws_iam_role_policy_attachment" "create_session" {
  role       = aws_iam_role.create_session.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_cloudwatch_log_group" "create_session" {
  name              = "/aws/lambda/${aws_lambda_function.create_session.function_name}"
  retention_in_days = local.logs_retention_in_days
}
