import os
import urllib
import webapp2

class PageNotFound(webapp2.RequestHandler):
	def get(self):
		self.response.write('Page not found!')

app = webapp2.WSGIApplication([
	('/', PageNotFound)
], debug=True)