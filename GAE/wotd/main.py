import os
import urllib
import webapp2
from google.appengine.ext import ndb
import wotd
import wotd_uploader

class PageNotFound(webapp2.RequestHandler):
	def get(self):
		self.response.write('Page not found!')

app = webapp2.WSGIApplication([
	('/', PageNotFound),
	('/wotd', wotd.GetWOTD),
	('/wotd_uploader', wotd_uploader.UploadWOTD),
], debug=True)