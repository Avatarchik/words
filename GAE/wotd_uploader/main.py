import os
import urllib
import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

wotds =
[
	WOTD(0,"OUTVOTES","To defeat someone or something by having a larger number of votes."),
	WOTD(1,"REGISTRAR","Someone who is in charge of official records, for example in a city or a college."),
	WOTD(2,"WRESTLINGS","To fight someone by holding onto and pulling or pushing someone."),
	WOTD(3,"MEANED","To have a particular meaning or be used as a symbol or sign for something."),
	WOTD(4,"CYANIDINGS","A very strong poison."),
	WOTD(5,"INSURANCES","Protected by insurance."),
	WOTD(6,"ELBOW","The joint where your arm bends."),
	WOTD(7,"BARBES","The sharp curved point of a hook,arrow, etc. that prevents it from being easily pulled out."),
	WOTD(8,"APPAREL","Clothing – used especially by stores or the clothing industry."),
	WOTD(9,"REPRESENTS","A member of the House of Representatives, the lower House of Congress in the United States."),
]

class PageNotFound(webapp2.RequestHandler):
	def get(self):
		self.response.write('Page not found!')

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		self.response.write('got here')
		for wotd in wotds:
			self.response.write('wotd')
			self.response.write(wotd.word)
			wotd.put()

app = webapp2.WSGIApplication([
	('/', PageNotFound),
	('/wotd_uploader', UploadWOTD),
], debug=True)