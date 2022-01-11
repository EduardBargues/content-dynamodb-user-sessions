locals {
  session_token = "SessionToken"
  user_name     = "UserName"
}
resource "aws_dynamodb_table" "main" {
  name         = local.prefix
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = local.session_token

  attribute {
    name = local.session_token
    type = "S"
  }

  attribute {
    name = local.user_name
    type = "S"
  }

  ttl {
    attribute_name = "TTL"
    enabled        = true
  }

  global_secondary_index {
    name            = "${local.prefix}-tokens-by-user"
    hash_key        = local.user_name
    projection_type = "KEYS_ONLY"
  }
}
