import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

wotds_words = [
	"OUTVOTES",
	"REGISTRAR", 
	"WRESTLINGS",
	"MEANED",
	"CYANIDINGS",
	"INSURANCES",
	"ELBOW",
	"BARBES",
	"APPAREL",
	"REPRESENTS"
]

wotds_definitions = [
	"To defeat someone or something by having a larger number of votes.",
	"Someone who is in charge of official records, for example in a city or a college.",
	"To fight someone by holding onto and pulling or pushing someone.",
	"To have a particular meaning or be used as a symbol or sign for something.",
	"A very strong poison.",
	"Protected by insurance.",
	"The joint where your arm bends.",
	"The sharp curved point of a hook,arrow, etc. that prevents it from being easily pulled out.",
	"Clothing – used especially by stores or the clothing industry.",
	"A member of the House of Representatives, the lower House of Congress in the United States."
]

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		requestedIndex = int(self.request.get('index', -1))

		if requestedIndex != -1:
			self.response.write(wotds_words[requestedIndex])
			self.response.write(wotds_definitions[requestedIndex])
			return

		self.response.write('FAILED')

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)