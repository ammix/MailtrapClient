# MailtrapClient
Create an extendable client with a method that will send an email using the Mailtrap Email Sending API https://api-docs.mailtrap.io/docs/mailtrap-api-docs/. The send() method should be able to receive a number of parameters, some of them optional.

For this task, the method should work when being called with the following parameters:
+ sender name and email
+ recipient name and email
+ subject
+ text
+ html
+ attachments

Highly recommended, create unit tests to cover the functionality available. 
Optionally package the client in a standalone library that is ready for distribution.

## Additional information:
+ You can create a free Mailtrap Email Sending account (to use for smoke testing) at https://mailtrap.io/register/signup
+ You can see examples of how such a method would be called (in either Ruby of NodeJS) at:
	- https://github.com/railsware/mailtrap-ruby
	- https://github.com/railsware/mailtrap-nodejs
