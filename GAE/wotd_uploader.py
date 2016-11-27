import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		WOTD(daystamp=0, word='dog animal', definition='goes bark').put()
		WOTD(daystamp=1, word='cat animal', definition='meows and owns things').put()
		WOTD(daystamp=2, word='bird animal', definition='flys places').put()
		WOTD(daystamp=3, word='octopus animal', definition='squishes in the sea').put()

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)