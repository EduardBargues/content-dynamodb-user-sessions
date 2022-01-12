resource "aws_api_gateway_method" "delete_user_sessions" {
  rest_api_id   = aws_api_gateway_rest_api.api.id
  resource_id   = aws_api_gateway_resource.sessions.id
  http_method   = "DELETE"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "delete_user_sessions" {
  rest_api_id             = aws_api_gateway_rest_api.api.id
  resource_id             = aws_api_gateway_resource.sessions.id
  http_method             = aws_api_gateway_method.delete_user_sessions.http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.delete_user_sessions.invoke_arn
}

resource "aws_lambda_permission" "delete_user_sessions" {
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.delete_user_sessions.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "arn:aws:execute-api:${var.aws_region}:${var.aws_account_id}:${aws_api_gateway_rest_api.api.id}/*/${aws_api_gateway_method.delete_user_sessions.http_method}${aws_api_gateway_resource.sessions.path}"
}
