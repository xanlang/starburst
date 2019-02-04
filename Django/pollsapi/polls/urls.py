from django.urls import path
from .apiviews import ChoiceList, CreateVote
from rest_framework.routers import DefaultRouter
from .apiviews import PollViewSet, UserCreate, LoginView, LogoutView

from rest_framework.authtoken import views

router = DefaultRouter()
router.register('polls', PollViewSet, base_name = 'polls')

urlpatterns = [
     path("polls/<int:pk>/choices/", ChoiceList.as_view(), name = "choice_list"),
     path("polls/<int:pk>/choices/<int:choice_pk>/vote/", CreateVote.as_view(), name="create_vote"),
     path("users/",UserCreate.as_view(), name="user_create"),
     path("login/",LoginView.as_view(), name="login"),
     path("login2/", views.obtain_auth_token, name="login2"), #another login endpoint
     #path("polls/", PollList.as_view(), name="polls_list"),
     #path("polls/<int:pk>/", PollDetail.as_view(), name="polls_detail"),
        ]

urlpatterns += router.urls
