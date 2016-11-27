import webapp2
from google.appengine.ext import ndb

class WOTD(ndb.Model):
	daystamp = ndb.IntegerProperty()
	word = ndb.StringProperty()
	definition = ndb.StringProperty()

class UploadWOTD(webapp2.RequestHandler):
	def get(self):
		WOTD(daystamp=0, word='Masticator', definition='A type of glue that does not crack or break when it is bent.').put()
		WOTD(daystamp=1, word='Vitals', definition='Vital signs.').put()
		WOTD(daystamp=2, word='Venerate', definition='Passed from one person to another during sex.').put()
		WOTD(daystamp=3, word='Neaten', definition='To make something neater and more organized.').put()
		WOTD(daystamp=4, word='Feministic', definition='Someone who supports the idea that women should have the same rights and opportunities as men.').put()
		WOTD(daystamp=5, word='Airplane', definition='A vehicle that flies by using wings and one or more engines.').put()
		WOTD(daystamp=6, word='Promulged', definition='To spread an idea or belief to as many people as possible.').put()
		WOTD(daystamp=7, word='Minuter', definition='Extremely small.').put()
		WOTD(daystamp=8, word='Quiting', definition='Very, but not extremely.').put()
		WOTD(daystamp=9, word='Shirk', definition='To deliberately avoid doing something you should do, because you are lazy.').put()

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)