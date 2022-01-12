locals {
  delete_user_sessions_file_name = "DeleteUserSessions.zip"
}
resource "aws_lambda_function" "delete_user_sessions" {
  function_name    = "${local.prefix}-delete-user-sessions"
  filename         = local.delete_user_sessions_file_name
  source_code_hash = filebase64sha256(local.delete_user_sessions_file_name)
  handler          = "DeleteUserSessions::DeleteUserSessions.Function::Handler"
  runtime          = "dotnetcore3.1"
  memory_size      = 256
  timeout          = 30
  role             = aws_iam_role.delete_user_sessions.arn

  environment {
    variables = {
      DYNAMODB_TABLE_NAME = aws_dynamodb_table.main.name
    }
  }
}

resource "aws_iam_role" "delete_user_sessions" {
  name = "${local.prefix}-delete-user-sessions"

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

resource "aws_iam_role_policy" "delete_user_sessions" {
  name = aws_lambda_function.delete_user_sessions.function_name
  role = aws_iam_role.delete_user_sessions.id

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [{
      "Effect" : "Allow",
      "Action" : [
        "dynamodb:Query",
        "dynamodb:DeleteItem"
      ],
      "Resource" : ["${aws_dynamodb_table.main.arn}", "${aws_dynamodb_table.main.arn}/*"]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "delete_user_sessions" {
  role       = aws_iam_role.delete_user_sessions.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_cloudwatch_log_group" "delete_user_sessions" {
  name              = "/aws/lambda/${aws_lambda_function.delete_user_sessions.function_name}"
  retention_in_days = local.logs_retention_in_days
}
