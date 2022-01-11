variable "service_name" {
  type        = string
  description = "name of the service."
  default     = "dynamodb-user-session"
}
variable "aws_region" {
  type        = string
  description = "aws region"
  sensitive   = true
}
variable "aws_account_id" {
  type        = string
  description = "aws account id"
  sensitive   = true
}
