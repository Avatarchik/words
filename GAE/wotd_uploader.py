import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)