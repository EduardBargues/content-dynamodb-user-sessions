resource "aws_api_gateway_resource" "sessions" {
  path_part   = "sessions"
  parent_id   = aws_api_gateway_rest_api.api.root_resource_id
  rest_api_id = aws_api_gateway_rest_api.api.id
}
