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
		WOTD(daystamp=10, word='Sniggerers', definition='Snicker.').put()
		WOTD(daystamp=11, word='Polytheism', definition='The belief that there is more than one god.').put()
		WOTD(daystamp=12, word='Adaptable', definition='A movie or play that was first written in a different form, for example as a book.').put()
		WOTD(daystamp=13, word='Lavish', definition='To give someone a lot of something such as expensive presents, love, or praise.').put()
		WOTD(daystamp=14, word='Mealed', definition='An occasion when you eat food, for example breakfast or dinner, or the food that you eat on that occasion.').put()
		WOTD(daystamp=15, word='Obstetric', definition='The part of medical science that deals with the birth of children.').put()
		WOTD(daystamp=16, word='Outvoting', definition='To defeat someone or something by having a larger number of votes.').put()
		WOTD(daystamp=17, word='Cohesible', definition='The quality a group of people, a set of ideas, etc. has when all the parts or members of it are connected or related in a reasonable way to form a whole.').put()
		WOTD(daystamp=18, word='Elections', definition='The person who has been elected as president, etc., but who has not yet officially started his or her job.').put()
		WOTD(daystamp=19, word='Singings', definition='To produce musical sounds, songs, etc. with your voice.').put()
		WOTD(daystamp=20, word='Apoplexy', definition='An illness caused by a problem in your brain that can damage your ability to move, feel, or think.').put()
		WOTD(daystamp=21, word='Anionic', definition='An ionatom or group of atoms with an electrical charge with a negative electrical charge that is attracted to the anode inside a battery, electrolytic cell, etc.').put()
		WOTD(daystamp=22, word='Sutures', definition='The act of sewing a wound together, or a stitch used to do this.').put()
		WOTD(daystamp=23, word='Skid', definition='A sudden sliding movement of a vehicle, that you cannot control.').put()
		WOTD(daystamp=24, word='Muffineers', definition='A small slightly sweet type of bread that often has fruit in it.').put()
		WOTD(daystamp=25, word='Hurtful', definition='Suffering pain or injury.').put()
		WOTD(daystamp=26, word='Err', definition='To be more careful, safe, etc. than is necessary rather than risk making a mistake.').put()
		WOTD(daystamp=27, word='Tamoxifen', definition='A drug that is used to treat breast cancer.').put()
		WOTD(daystamp=28, word='Unisex', definition='Intended for both men and women.').put()
		WOTD(daystamp=29, word='Melled', definition='Running or moving in a fast uncontrolled way.').put()
		WOTD(daystamp=30, word='Backroom', definition='Backroom deals, politics, etc. happen in a private or secret way, when they should happen in public.').put()

app = webapp2.WSGIApplication([
	('/wotd_uploader/?', UploadWOTD),
], debug=True)