resource "aws_api_gateway_resource" "session_id" {
  path_part   = "{sessionId}"
  parent_id   = aws_api_gateway_resource.sessions.id
  rest_api_id = aws_api_gateway_rest_api.api.id
}

resource "aws_api_gateway_method" "get_session_by_token" {
  rest_api_id   = aws_api_gateway_rest_api.api.id
  resource_id   = aws_api_gateway_resource.session_id.id
  http_method   = "GET"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "get_session_by_token" {
  rest_api_id             = aws_api_gateway_rest_api.api.id
  resource_id             = aws_api_gateway_resource.session_id.id
  http_method             = aws_api_gateway_method.get_session_by_token.http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.get_session_by_token.invoke_arn
}

resource "aws_lambda_permission" "get_session_by_token" {
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.get_session_by_token.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "arn:aws:execute-api:${var.aws_region}:${var.aws_account_id}:${aws_api_gateway_rest_api.api.id}/*/${aws_api_gateway_method.get_session_by_token.http_method}${aws_api_gateway_resource.session_id.path}"
}
