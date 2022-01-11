locals {
  get_session_by_token_file_name = "GetSessionByToken.zip"
}
resource "aws_lambda_function" "get_session_by_token" {
  function_name    = "${local.prefix}-get-session-by-token"
  filename         = local.get_session_by_token_file_name
  source_code_hash = filebase64sha256(local.get_session_by_token_file_name)
  handler          = "GetSessionByToken::GetSessionByToken.Function::Handler"
  runtime          = "dotnetcore3.1"
  memory_size      = 256
  timeout          = 30
  role             = aws_iam_role.get_session_by_token.arn

  environment {
    variables = {
      DYNAMODB_TABLE_NAME = aws_dynamodb_table.main.name
    }
  }
}

resource "aws_iam_role" "get_session_by_token" {
  name = "${local.prefix}-get-session-by-token"

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

resource "aws_iam_role_policy" "get_session_by_token_dynamodb" {
  name = aws_lambda_function.get_session_by_token.function_name
  role = aws_iam_role.get_session_by_token.id

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [{
      "Effect" : "Allow",
      "Action" : [
        "dynamodb:GetItem"
      ],
      "Resource" : "${aws_dynamodb_table.main.arn}"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "get_session_by_token" {
  role       = aws_iam_role.get_session_by_token.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_cloudwatch_log_group" "get_session_by_token" {
  name              = "/aws/lambda/${aws_lambda_function.get_session_by_token.function_name}"
  retention_in_days = local.logs_retention_in_days
}
