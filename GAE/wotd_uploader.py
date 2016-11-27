import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

wotds = [
	0,
	1,
	2,
	3
]

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		requestedIndex = int(self.request.get('index', -1))

		if requestedIndex != -1:
			self.response.write(wotds[requestedIndex])
			return

		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)