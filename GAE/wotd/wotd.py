import os
import urllib
import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class GetWOTD(webapp2.RequestHandler):
	def get(self):
		daystamp = int(self.request.get('daystamp', -1))

		if daystamp != -1:
			todays_wotd = WOTD.query(WOTD.daystamp == daystamp).get()
			if todays_wotd:
				self.response.write(todays_wotd.daystamp)
				self.response.write('##')
				self.response.write(todays_wotd.word)
				self.response.write('##')
				self.response.write(todays_wotd.definition)
				return
		
		self.response.write('UNKNOWN')

app = webapp2.WSGIApplication([
	('/wotd/?', GetWOTD),
], debug=True)